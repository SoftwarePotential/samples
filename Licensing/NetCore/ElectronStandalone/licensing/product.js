/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */
const EdgePromiseFactory = require('../edgePromiseFactory');

class Product {
    constructor() {
        const edgePromise = new EdgePromiseFactory('ImageEditor.Core.dll', 'ImageEditor.Core.Licensing');

        this.getName = edgePromise.create('GetProductName');
        this.getVersion = edgePromise.create('GetProductVersion');
        this.getEdition = edgePromise.create('GetEdition');
        this.getFeatures = edgePromise.create('GetFeatures');
    }
}

module.exports = Product;