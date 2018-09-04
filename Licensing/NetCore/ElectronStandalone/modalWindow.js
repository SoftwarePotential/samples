const path = require('path');
const url = require('url');
const UndefinedArgumentError = require('./undefinedArgumentError');
const {
    BrowserWindow,
} = require('electron');

_window = new WeakMap();
_parent = new WeakMap();
_isDevToolsVisible = new WeakMap();

class ModalWindow {
    constructor(parent = UndefinedArgumentError.throwFor('parent')) {
        _parent.set(this, parent);
        _isDevToolsVisible.set(this, false);
    }

    open(view) {
        const parent = _parent.get(this);

        _window.set(this, new BrowserWindow({
            parent: parent,
            modal: true,
            frame: false,
            show: false,
            backgroundColor: '#333',
            width: 480,
            minHeight: 768,
            center: false,
            x: parent.getContentBounds().x,
            y: parent.getContentBounds().y,
            height: parent.getContentSize()[1],
        }));

        let window = _window.get(this);
        window.on('closed', () => window = null);

        this.loadHtml(view);

        if (_isDevToolsVisible.get(this)) window.webContents.openDevTools();
        window.once('ready-to-show', window.show);
    }

    loadHtml(view) {
        _window.get(this).loadURL(url.format({
            pathname: path.join(__dirname, view),
            protocol: 'file:',
            slashes: true
        }));
    }

    toggleDevTools() {
        _isDevToolsVisible.set(this, !_isDevToolsVisible.get(this));
    }
}

module.exports = ModalWindow;