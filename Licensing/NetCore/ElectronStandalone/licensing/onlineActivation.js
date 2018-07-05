/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */
const {
    ipcRenderer,
} = require('electron');
const Messages = require('../messages');
const Activation = require('./activationService');
const activation = new Activation();

const fontawesome = require('@fortawesome/fontawesome');
const faSpinner = require('@fortawesome/fontawesome-free-solid/faSpinner');
fontawesome.library.add(faSpinner);

let onlineKey;
let activateButton;
let spinner;
let messages;
let closeButton;

document.addEventListener('DOMContentLoaded', () => {
    onlineKey = document.getElementById('onlineKey');
    activateButton = document.getElementById('activate');
    spinner = document.getElementById('spinner');
    messages = new Messages(document.getElementById('messages'));
    closeButton = new Messages(document.getElementsByClassName('closeButton')[0]);

    onlineKey.oninput = onOnlineKeyInput;
    activateButton.onclick = onActivateClicked;
});

async function onOnlineKeyInput() {
    messages.clear();
    closeButton.clear();

    const key = onlineKey.value;
    const isEmpty = string => string.trim().length === 0;
    if (isEmpty(key)) return;

    try {
        onKeyValidated(await activation.isActivationKeyWellFormed(onlineKey.value));
    } catch (err) {
        console.log(err);
    }
}

function onKeyValidated(isValid) {
    if (isValid) activateButton.disabled = false;
    else {
        activateButton.disabled = true;
        messages.updateWithError('Activation key is not in the correct format.');
        closeButton.updateWithError();
    }
}

async function onActivateClicked() {
    spinner.style.display = 'inline-block';

    try {
        await activation.activate(onlineKey.value);
        messages.updateWithSuccess('Activation Complete!');
        closeButton.updateWithSuccess();
        ipcRenderer.send('license-update');
        spinner.style.display = 'none';
    } catch (err) {
        messages.updateWithError(err['Message']);
        closeButton.updateWithError();
    }
}