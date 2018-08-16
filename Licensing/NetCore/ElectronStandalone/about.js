const {
    ipcRenderer,
    remote
} = require('electron');

const Product = require('./licensing/product');
const product = new Product();
const Messages = require('./messages');

let productName;
let productVersion;
let productEdition;
let licenseStatusLink;
let licenseActivationLink;
let messages;

document.addEventListener('DOMContentLoaded', () => {
    productName = document.getElementById('productName');
    productVersion = document.getElementById('productVersion');
    productEdition = document.getElementById('productEdition');
    licenseStatusLink = document.getElementById('openLicenseStatus');
    licenseActivationLink = document.getElementById('openLicenseActivation');

    licenseStatusLink.onclick = () => ipcRenderer.send('open-license-status');;
    licenseActivationLink.onclick = () => ipcRenderer.send('open-license-activation');
    messages = new Messages(document.getElementById('messages'));

    setProductName();
    setProductVersion();
    setEdition();

    document.getElementsByClassName('closeIcon')[0].onclick = () => remote.BrowserWindow.getFocusedWindow().close();
    document.getElementsByClassName('closeButton')[0].onclick = () => remote.BrowserWindow.getFocusedWindow().close();
});

async function setProductName() {
    try {
        productName.innerText = await product.getName();
    } catch (err) {
        console.log(err);
    }
}

async function setProductVersion() {
    try {
        productVersion.innerText = await product.getVersion();
    } catch (err) {
        console.log(err);
    }
}

async function setEdition() {
    try {
        const edition = await product.getEdition();

        if (edition === null) {
            licenseActivationLink.style.display = 'none';
        } else if (edition !== 'Unlicensed') {
            productEdition.innerText = `${await product.getEdition()} Edition`;
            licenseActivationLink.style.display = 'none';
        } else {
            licenseStatusLink.style.display = 'none';
            messages.updateWithWarning('No active licenses.');
        }
    } catch (err) {
        console.log(err);
    }
}