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
    remote
} = require('electron');
const Mustache = require('mustache');
const Store = require('./store');
const moment = require('moment');

const store = new Store();
let licenseList;
let licenseListTemplate;
let noLicenseTemplate;

document.addEventListener('DOMContentLoaded', () => {
    licenseList = document.getElementById('licenseList');
    licenseListTemplate = document.getElementById('licenseListTemplate').innerHTML;
    noLicenseTemplate = document.getElementById('noLicenseTemplate').innerHTML;

    document.getElementById('openLicenseActivation').onclick = () => ipcRenderer.send('open-license-activation');
    document.getElementsByClassName('closeIcon')[0].onclick = () => remote.BrowserWindow.getFocusedWindow().close();
    document.getElementsByClassName('closeButton')[0].onclick = () => remote.BrowserWindow.getFocusedWindow().close();

    updateLicenses();
});

async function updateLicenses() {
    try {
        const licenses = await store.retrieveAllLicenses();
        if (licenses.length == 0) licenseList.innerHTML = noLicenseTemplate;
        else renderLicenseList(licenses);
    } catch (err) {
        console.log(err);
    }
}

function renderLicenseList(licenses) {
    licenses.forEach(license => license['ValidUntil'] = moment(license['ValidUntil']).format('DD/MM/YYYY'));

    Mustache.parse(licenseListTemplate); // optional, speeds up future uses
    licenseList.innerHTML = Mustache.render(licenseListTemplate, {
        licenses: licenses
    });

    var deleteButtonsHtmlCollection = document.getElementsByClassName('deleteLicense');
    [].forEach.call(deleteButtonsHtmlCollection, button => button.onclick = onDeleteButtonClicked);
}

async function onDeleteButtonClicked(e) {
    try {
        await store.deleteLicenseByActivationKey(e.target.dataset.activationKey);
        updateLicenses();
        ipcRenderer.send('license-update');
    } catch (err) {
        console.log(err);
    }
}