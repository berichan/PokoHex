"use strict";

window.pokohexDrop = {
    folders: new WeakMap(),
    handlers: new WeakMap(),

    getPaths(input) {
        const folder = this.folders.get(input);
        if (folder?.files === input.files)
            return folder.paths;
        this.folders.delete(input);
        return Array.from(input.files, file => file.webkitRelativePath || file.name);
    },

    initialize(element, receiver, maximumFiles) {
        this.dispose(element);
        const controller = new AbortController();
        const options = { signal: controller.signal };
        this.handlers.set(element, controller);
        let depth = 0;
        let reading = false;

        const isFileDrag = event => Array.from(event.dataTransfer?.types || []).includes("Files");
        const isBusy = () => reading || element.getAttribute("aria-busy")?.toLowerCase() === "true";
        const clear = () => {
            depth = 0;
            element.classList.remove("file-drag");
        };

        const open = async (entries, files) => {
            reading = true;
            try {
                const selection = [];
                let input;
                if (entries.length === 1 && entries[0]?.isDirectory) {
                    await this.readDirectory(entries[0], "", selection, maximumFiles);
                    if (selection.length === 0)
                        throw new Error("The dropped folder is empty.");
                    input = document.getElementById("open-folder");
                }
                else if (files.length === 1 && !entries.some(entry => entry?.isDirectory) && /\.zip$/i.test(files[0].name)) {
                    selection.push({ file: files[0], path: files[0].name });
                    input = document.getElementById("open-zip");
                }
                else {
                    throw new Error("Drop one save folder or one ZIP file.");
                }

                if (controller.signal.aborted || element.getAttribute("aria-busy")?.toLowerCase() === "true")
                    return;
                if (typeof DataTransfer !== "function")
                    throw new Error("This browser cannot open dropped files. Use Open save folder or Open ZIP.");
                const transfer = new DataTransfer();
                for (const entry of selection)
                    transfer.items.add(entry.file);
                input.files = transfer.files;
                if (input.id === "open-folder")
                    this.folders.set(input, { files: input.files, paths: selection.map(entry => entry.path) });
                // The existing InputFile handlers own validation and the discard prompt.
                input.dispatchEvent(new Event("change", { bubbles: true }));
            }
            catch (error) {
                if (!controller.signal.aborted)
                    await receiver.invokeMethodAsync("DropFailed", error.message || "The dropped save could not be opened.");
            }
            finally {
                reading = false;
            }
        };

        document.addEventListener("dragenter", event => {
            if (!isFileDrag(event))
                return;
            event.preventDefault();
            if (!isBusy()) {
                depth++;
                element.classList.add("file-drag");
            }
        }, options);

        document.addEventListener("dragover", event => {
            if (!isFileDrag(event))
                return;
            event.preventDefault();
            event.dataTransfer.dropEffect = isBusy() ? "none" : "copy";
        }, options);

        document.addEventListener("dragleave", event => {
            if (isFileDrag(event) && --depth <= 0)
                clear();
        }, options);

        document.addEventListener("drop", event => {
            if (!isFileDrag(event))
                return;
            event.preventDefault();
            clear();
            if (isBusy())
                return;
            // Entries must be captured while the browser's drop data is readable.
            const items = Array.from(event.dataTransfer.items || []).filter(item => item.kind === "file");
            const entries = items.map(item => item.getAsEntry?.() ?? item.webkitGetAsEntry?.());
            const files = Array.from(event.dataTransfer.files);
            open(entries, files).catch(error => console.error(error));
        }, options);

        document.addEventListener("dragend", clear, options);
        window.addEventListener("blur", clear, options);
    },

    async readDirectory(entry, parent, selection, maximumFiles) {
        const path = parent ? `${parent}/${entry.name}` : entry.name;
        if (entry.isFile) {
            if (selection.length >= maximumFiles)
                throw new Error(`Select no more than ${maximumFiles} save files.`);
            const file = await new Promise((resolve, reject) => entry.file(resolve, reject));
            selection.push({ file, path });
            return;
        }
        const reader = entry.createReader();
        for (;;) {
            const entries = await new Promise((resolve, reject) => reader.readEntries(resolve, reject));
            if (entries.length === 0)
                break;
            for (const child of entries)
                await this.readDirectory(child, path, selection, maximumFiles);
        }
    },

    dispose(element) {
        this.handlers.get(element)?.abort();
        this.handlers.delete(element);
        element.classList.remove("file-drag");
    }
};
