const EdgePromiseFactory = require('./edgePromiseFactory');

class ImageEditorCore {
    constructor() {
        const edgePromise = new EdgePromiseFactory('ImageEditor.Core.dll', 'ImageEditor.Core.Features');

        this.convertToGreyscale = edgePromise.create('ConvertToGreyscale');
        this.crop = edgePromise.create('Crop');
        this.rotate = edgePromise.create('Rotate');
    }
}

module.exports = ImageEditorCore;