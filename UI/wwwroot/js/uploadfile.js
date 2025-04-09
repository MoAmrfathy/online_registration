

function fileInit(elementId, mimetypeAllowed = "", componentInstance) {
    filesAdded = new Map();
    mimetypesAllowed = mimetypeAllowed;
    document.querySelector(elementId).addEventListener('change', function () {
        const files = this.files;
        var invalidExists = false;
        if (this.multiple === false)
            filesAdded.clear();
        const mimetypes = mimetypeAllowed.split(",");
        if (files.length === 0) {
            componentInstance.invokeMethodAsync('SetAttachmentValidation', 0, FileNamesToStringArray()).then(null, function (err) {
            });
            return "";
        }
        for (let i = 0; i < files.length; i++) {
            let file = files[i];
            var isValid = -1;
            if (mimetypeAllowed !== "") {
                for (var j = 0; j < mimetypes.length; j++)
                    if (file.type.includes(mimetypes[j])) {
                        isValid = 1;
                        break;
                    }
                if (isValid !== -1) {
                    filesAdded.set(file.name, file);
                }
                else
                    invalidExists = true;

            }
        }
        if (invalidExists === true) {
            componentInstance.invokeMethodAsync('SetAttachmentValidation', -1, FileNamesToStringArray()).then(null, function (err) {
            });
        }
        else
            componentInstance.invokeMethodAsync('SetAttachmentValidation', 1, FileNamesToStringArray()).then(null, function (err) {
            });

    });
}

async function uploadFile(elementId, url, guids, folder) {
    //const files = document.querySelector(elementId).files;
    const formData = new FormData();
    var filenames = [];
    if (filesAdded.size === 0)
        return null;
    let i = 0;
    for (let file of filesAdded.values()) {
        //let file = files[i];

        formData.append('files', file);
        formData.append('guids', guids[i]);
        filenames.push(file.name);
        i++;
    }

    var response = await fetch(url + '?folder=' + folder + '&mimetypesallowed=' + mimetypesAllowed, {
        method: 'POST',
        body: formData
    });
    return {
        'filenames': filenames,
        'responseCode': response.status
    };
}








function fileInit2(elementId, mimetypeAllowed = "", componentInstance) {
    filesAdded = new Map();
    mimetypesAllowed = mimetypeAllowed;
    document.querySelector(elementId).addEventListener('change', function () {
        const files = this.files;
        const mimetypes = mimetypeAllowed.split(",");
        if (files.length === 0) {
            componentInstance.invokeMethodAsync('SetAttachmentValidation', 0, FileNamesToStringArray()).then(null, function (err) {
            });
            return "";
        }
        for (let i = 0; i < files.length; i++) {
            let file = files[i];
            if (mimetypeAllowed !== "") {
                var isValid = -1;
                for (var j = 0; j < mimetypes.length; j++)
                    if (file.type.includes(mimetypes[j])) {
                        isValid = 1;
                        break;
                    }
                componentInstance.invokeMethodAsync('SetAttachmentValidation', isValid).then(null, function (err) {
                });
                if (isValid === -1) {
                    componentInstance.invokeMethodAsync('AttachmentValidation').then(null, function (err) { });
                    componentInstance.invokeMethodAsync('ClearAttachment').then(null, function (err) { });
                    break;
                }
                else {
                    filesAdded.set(file.name, file);
                }
            }
        }
        document.getElementById('CurrentAttachment').innerHTML = "";
        for (let file of filesAdded.values()) {
            document.getElementById('CurrentAttachment').innerHTML += " ' " + file.name + " '  ";
        }
    });
}

async function uploadFile2(elementId, url, guids, folder) {
    const formData = new FormData();
    var filenames = [];
    if (filesAdded.size === 0)
        return null;
    for (let file of filesAdded.values()) {
        formData.append('files', file);
        formData.append('guids', guids[0]);
        filenames.push(file.name);
    }
    var response = await fetch(url + '?folder=' + folder + '&mimetypesallowed=' + mimetypesAllowed, {
        method: 'POST',
        body: formData
    });
    return {
        'filenames': filenames,
        'responseCode': response.status
    };
}

function clearFile(elementId) {
    const ev = new Event("change");
    document.querySelector(elementId).value = "";
    document.querySelector(elementId).dispatchEvent(ev);
    filesAdded.clear();
}

function clearFile2(elementId) {
    const ev = new Event("change");
    document.querySelector(elementId).value = "";
    document.querySelector(elementId).dispatchEvent(ev);
    filesAdded.clear();
    document.getElementById('CurrentAttachment').innerHTML = "";
}


function FileNamesToStringArray() {
    var keys = [];
    for (let key of filesAdded.keys()) {
        keys.push(key);
    }
    return keys;
}

function RemoveFile(fileName) {
    filesAdded.delete(fileName);
}