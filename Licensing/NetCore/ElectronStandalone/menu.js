const UndefinedArgumentError = require('./undefinedArgumentError');
const {
    Menu,
    MenuItem,
    ipcMain
} = require('electron'); // this has to be after setting EDGE_USE_CORECLR and EDGE_APP_ROOT
const Product = require('./licensing/product');
const product = new Product();

_template = new WeakMap();
_update = new WeakMap();
_addLicensedMenuItems = new WeakMap();
_createMenuItemFromLabel = new WeakMap();

class ImageEditorMenu {
    constructor(
        mainWindow = UndefinedArgumentError.throwFor('mainWindow'),
        modalWindow = UndefinedArgumentError.throwFor('modalWindow')
    ) {
        _template.set(this, [{
                label: 'File',
                submenu: [{
                        label: 'Open File...',
                        accelerator: 'CommandOrControl+O',
                        click() {
                            mainWindow.webContents.send('open-ofd');
                        }
                    },
                    {
                        role: 'quit'
                    }
                ]
            },
            {
                label: 'Edit',
                submenu: [{
                        label: 'Cut',
                        accelerator: 'CmdOrCtrl+X',
                        selector: 'cut:'
                    },
                    {
                        label: 'Copy',
                        accelerator: 'CmdOrCtrl+C',
                        selector: 'copy:'
                    },
                    {
                        label: 'Paste',
                        accelerator: 'CmdOrCtrl+V',
                        selector: 'paste:'
                    }
                ],
                id: 'edit'
            },
            {
                label: 'Help',
                submenu: [{
                        label: 'View Licenses',
                        click() {
                            modalWindow.open('licensing/licenseStatus.html');
                        }
                    },
                    {
                        label: 'Toggle Developer Tools',
                        click() {
                            mainWindow.webContents.toggleDevTools();
                            modalWindow.toggleDevTools();
                        }
                    }, {
                        type: 'separator'
                    }, {
                        label: 'About',
                        click() {
                            modalWindow.open('about.html');
                        }
                    }
                ]
            }
        ]);

        _update.set(this, async () => {
            const menu = Menu.buildFromTemplate(_template.get(this));

            try {
                const features = await product.getFeatures();
                if (features.length > 0) _addLicensedMenuItems.get(this)(menu, features);

            } catch (err) {
                console.log(err);
            }

            Menu.setApplicationMenu(menu);
        });

        _addLicensedMenuItems.set(this, (menu, features) => {
            const createItem = _createMenuItemFromLabel.get(this);

            const editSubmenu = menu.items[1].submenu;
            editSubmenu.append(new MenuItem({
                type: 'separator'
            }));

            if (features.includes('Feature1')) editSubmenu.append(createItem('Convert To Greyscale'));
            if (features.includes('Feature2')) {
                editSubmenu.append(createItem('Rotate CW'));
                editSubmenu.append(createItem('Rotate CCW'));
            }
            if (features.includes('Feature3')) editSubmenu.append(createItem('Crop'));
        });

        _createMenuItemFromLabel.set(this, label => {
            const eventId = label.toLowerCase().replace(/ /g, '-');

            return new MenuItem({
                label: label,
                click() {
                    mainWindow.webContents.send(eventId);
                },
                enabled: false
            });
        });

        ipcMain.on('enable-edit', () => {
            const menu = Menu.getApplicationMenu();
            const edit = menu.getMenuItemById('edit');
            if (edit) edit.submenu.items.forEach(item => item.enabled = true);
        });
        ipcMain.on('license-update', _update.get(this));
    }

    set() {
        _update.get(this)();
    }
}

module.exports = ImageEditorMenu;