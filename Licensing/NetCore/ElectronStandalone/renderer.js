const fs = require('fs');
const {
    ipcRenderer,
    remote
} = require('electron');
const LicenseStore = require('./licensing/store');
const ImageEditorCore = require('./imageEditorCore');
const Marquee = require('./marquee');
const imageEditorCore = new ImageEditorCore();

let selectedImage;
let selectedFilename;
let marquee;

document.addEventListener('DOMContentLoaded', () => {
    selectedImage = document.getElementById('selectedImage');
    selectedFilename = document.getElementById('selectedFilename');

    createCropMarquee();
});

function createCropMarquee() {
    canvas = document.getElementById('canvas');
    canvas.width = selectedImage.width;
    canvas.height = 0;
    marquee = new Marquee(canvas, document.getElementById('marqueeInfo'));

    selectedImage.onload = () => {
        canvas.style.height = selectedImage.height + 'px';
        canvas.height = selectedImage.height;
        marquee.reset();
    };
}

ipcRenderer.on('open-ofd', openOfd);
ipcRenderer.on('convert-to-greyscale', convertToGreyscale);
ipcRenderer.on('rotate-cw', rotateCW);
ipcRenderer.on('rotate-ccw', rotateCCW);
ipcRenderer.on('crop', crop);

function openOfd() {
    remote.dialog.showOpenDialog({
        defaultPath: remote.app.getPath('pictures'),
        filters: [{
            name: 'Images',
            extensions: ['jpg', 'jpeg']
        }]
    }, onFileSelected);
}

function onFileSelected(filenames) {
    if (filenames === undefined) return;

    const path = filenames[0];
    fs.readFile(path, (err, data) => {
        if (err) {
            console.log(err);
            return;
        };

        updateSelectedImageDataUrl(Buffer.from(data).toString('base64'));
        selectedFilename.innerText = path;
        ipcRenderer.send('enable-edit');
    });
}

async function convertToGreyscale() {
    try {
        updateSelectedImageDataUrl(await imageEditorCore.convertToGreyscale({
            base64String: getSelectedImageData()
        }));
    } catch (err) {
        console.log(err);
    }
};

function rotateCW() {
    rotate({
        base64String: getSelectedImageData(),
        isClockwise: true
    });
}

function rotateCCW() {
    rotate({
        base64String: getSelectedImageData(),
        isClockwise: false
    });
}

async function rotate(options) {
    try {
        updateSelectedImageDataUrl(await imageEditorCore.rotate(options));
    } catch (err) {
        console.log(err);
    }
}

async function crop() {
    try {
        updateSelectedImageDataUrl(await imageEditorCore.crop({
            base64String: getSelectedImageData(),
            offsetX: marquee.x,
            offsetY: marquee.y,
            originalWidth: selectedImage.width,
            originalHeight: selectedImage.height,
            width: marquee.width,
            height: marquee.height
        }));
    } catch (err) {
        console.log(err);
    }
};

function getSelectedImageData() {
    return toBase64String(selectedImage.src);
}

function updateSelectedImageDataUrl(base64String) {
    return selectedImage.src = toDataUrl(base64String);
}

const dataUrlPrefix = 'data:image/jpeg;base64,';

function toBase64String(dataUrl) {
    return dataUrl.replace(dataUrlPrefix, '');
}

function toDataUrl(base64String) {
    return dataUrlPrefix + base64String;
}

// this must be called before any other licensing calls and in the same process.
new LicenseStore().verifyStoresInitialized();