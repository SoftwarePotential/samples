/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */
const EdgePromiseFactory = require('../edgePromiseFactory');

class Activation {
    constructor() {
        const edgePromise = new EdgePromiseFactory('ImageEditor.Core.dll', 'ImageEditor.Core.Licensing');

        this.activate = edgePromise.create('Activate');
        this.isActivationKeyWellFormed = edgePromise.create('IsActivationKeyWellFormed');
        this.generateActivationRequest = edgePromise.create('GenerateActivationRequest');
        this.installLicenseFile = edgePromise.create('InstallLicenseFile');
    }

    static get licenseRevisionException() {
        return 'Sp.Agent.Licensing.LicenseRevisionException';
    }

    static get nonmatchingProductIdException() {
        return 'Sp.Agent.Storage.NonmatchingProductIdException';
    }
}

module.exports = Activation;