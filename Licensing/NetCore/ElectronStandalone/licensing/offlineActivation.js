/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */
const moment = require('moment');
const fs = require('fs');
const path = require('path');
const {
    remote
} = require('electron');
const {
    app,
    dialog,
} = remote;
const Activation = require('./activationService');
const Product = require('./product');
const Messages = require('../messages');

const activation = new Activation();
const product = new Product();
let offlineKey;
let generateButton;
let install;
let closeButton;

document.addEventListener('DOMContentLoaded', () => {
    offlineKey = document.getElementById('offlineKey');
    generateButton = document.getElementById('generate');
    install = document.getElementById('install');
    closeButton = new Messages(document.getElementsByClassName('closeButton')[0]);
    messages = new Messages(document.getElementById('messages'));

    offlineKey.oninput = onOfflineKeyInput;
    generateButton.onclick = onGenerateClicked;
    install.onclick = onInstallClicked;
});

async function onOfflineKeyInput() {
    messages.clear();
    closeButton.clear();

    const isEmpty = string => string.trim().length === 0;
    if (isEmpty(offlineKey.value)) return;

    try {
        const isValid = await activation.isActivationKeyWellFormed(offlineKey.value);
        onKeyValidated(isValid);
    } catch (err) {
        console.log(err);
    }
}

function onKeyValidated(isValid) {
    if (isValid) generateButton.disabled = false;
    else {
        generateButton.disabled = true;
        updateMessagesWithError('Activation key is not in the correct format.');
    }
}

async function onGenerateClicked() {
    try {
        const request = await activation.generateActivationRequest(offlineKey.value);
        saveRequestFile(offlineKey.value, request);
    } catch (err) {
        updateMessagesWithError(err['Message']);
    }
}

function saveRequestFile(key, requestString) {
    dialog.showSaveDialog({
        defaultPath: path.join(
            app.getPath('desktop'),
            `${key}_${moment().format('YYYY-MM-DD_HH-mm-SSSS')}.txt`),
        filters: [{
            name: 'Text Files',
            extensions: ['txt']
        }]
    }, filename => {
        if (filename === undefined) return;

        fs.writeFile(filename, requestString, err => {
            if (err) updateMessagesWithError('Could not save Activation Request File. ' + err.message);
            else updateMessagesWithSuccess(`Activation Request File saved at ${filename}.`);
        });
    });
}

function onInstallClicked() {
    dialog.showOpenDialog({
        defaultPath: app.getPath('downloads'),
        filters: [{
            name: 'License Files',
            extensions: ['bin']
        }]
    }, onFileSelected);
}

async function onFileSelected(filenames) {
    if (filenames === undefined) return;

    try {
        await activation.installLicenseFile(filenames[0]);
        updateMessagesWithSuccess('The license has been successfully installed.');
    } catch (err) {
        handleInstallError(err);
    }
};

function handleInstallError(err) {
    if (err['Name'] === Activation.licenseRevisionException) updateMessagesWithError('There is a newer version of the license already installed.');
    else if (err['Name'] === Activation.nonmatchingProductIdException)
        updateMessagesWithError(err.message);
    else console.log(err);
}

function updateMessagesWithError(message) {
    messages.updateWithError(message);
    closeButton.updateWithError();
}

function updateMessagesWithSuccess(message) {
    messages.updateWithSuccess(message);
    closeButton.updateWithSuccess();
}