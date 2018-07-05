/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */
const EdgePromiseFactory = require('../edgePromiseFactory');

class Store {
    constructor() {
        const edgePromise = new EdgePromiseFactory('ImageEditor.Core.dll', 'ImageEditor.Core.Licensing');

        this.verifyStoresInitialized = async () => {
            try {
                await edgePromise.create('VerifyStoresInitialized')();
                console.log('License store initialization verified.');
            } catch (err) {
                console.log(err);
            }
        };
        this.retrieveAllLicenses = edgePromise.create('RetrieveAllLicenses');
        this.deleteLicenseByActivationKey = edgePromise.create('DeleteLicenseByActivationKey');
    }

    static get notLicensedExceptionName() {
        return 'Sp.Agent.Execution.NotLicensedException';
    }
}

module.exports = Store;