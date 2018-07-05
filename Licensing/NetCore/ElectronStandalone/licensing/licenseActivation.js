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
const Messages = require('../messages');

document.addEventListener('DOMContentLoaded', () => {
    setTabs([{
            tab: document.getElementById('online'),
            panel: document.getElementById('onlinePanel')
        },
        {
            tab: document.getElementById('offline'),
            panel: document.getElementById('offlinePanel')
        }
    ]);

    document.getElementById('openLicenseStatus').onclick = () => ipcRenderer.send('open-license-status');
    document.getElementsByClassName('closeIcon')[0].onclick = () => remote.BrowserWindow.getFocusedWindow().close();
    document.getElementsByClassName('closeButton')[0].onclick = () => remote.BrowserWindow.getFocusedWindow().close();
});

function setTabs(tabs) {
    tabs.forEach(t => t.tab.onclick = () => {
        const messages = new Messages(document.getElementById('messages'));
        const closeButton = new Messages(document.getElementsByClassName('closeButton')[0]);
        messages.clear();
        closeButton.clear();

        tabs.forEach(t => {
            t.tab.classList.remove('selected');
            t.panel.classList.remove('selected');
        });
        t.panel.classList.add('selected');
        t.tab.classList.add('selected');
    });
}