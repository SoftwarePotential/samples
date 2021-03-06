const path = require('path');
const url = require('url');
const {
  platform
} = require('os');
const {
  app,
  BrowserWindow,
  ipcMain
} = require('electron'); // this has to be after setting EDGE_USE_CORECLR and EDGE_APP_ROOT
const ModalWindow = require('./modalWindow');
const ImageEditorMenu = require('./menu');
const Product = require('./licensing/product');

// Keep a global reference of the window objects, if you don't, the window will
// be closed automatically when the JavaScript object is garbage collected.
let mainWindow;
let modalWindow;

ipcMain.on('open-license-status', () => modalWindow.loadHtml('licensing/licenseStatus.html'));
ipcMain.on('open-license-activation', () => modalWindow.loadHtml('licensing/licenseActivation.html'));

function createWindow() {
  // Create the browser window.
  const extension = platform() === 'win32' ? '.ico' : '.png';
  const iconFile = 'camera-retro-blue' + extension;
  mainWindow = new BrowserWindow({
    width: 1024,
    height: 768,
    backgroundColor: '#333',
    icon: path.join(__dirname, 'style', iconFile),
    show: false,
    webPreferences: {
      nodeIntegration: true
  }
  });

  modalWindow = new ModalWindow(mainWindow);
  new ImageEditorMenu(mainWindow, modalWindow).set();

  // and load the index.html of the app.
  mainWindow.loadURL(url.format({
    pathname: path.join(__dirname, 'index.html'),
    protocol: 'file:',
    slashes: true
  }));

  openAboutIfUnlicensed();

  mainWindow.once('ready-to-show', mainWindow.show);

  // Emitted when the window is closed.
  mainWindow.on('closed', function () {
    // Dereference the window object, usually you would store windows
    // in an array if your app supports multi windows, this is the time
    // when you should delete the corresponding element.
    mainWindow = null;
    modalWindow = null;
  });
}

async function openAboutIfUnlicensed() {
  try {
    edition = await new Product().getEdition();;
    if (edition === 'Unlicensed') modalWindow.open('about.html');
  } catch (err) {
    console.log(err);
  }
}

// This method will be called when Electron has finished
// initialization and is ready to create browser windows.
// Some APIs can only be used after this event occurs.
app.on('ready', createWindow);

// Quit when all windows are closed.
app.on('window-all-closed', function () {
  // On OS X it is common for applications and their menu bar
  // to stay active until the user quits explicitly with Cmd + Q
  if (process.platform !== 'darwin') {
    app.quit();
  }
});

app.on('activate', function () {
  // On OS X it's common to re-create a window in the app when the
  // dock icon is clicked and there are no other windows open.
  if (mainWindow === null) {
    createWindow();
  }
});