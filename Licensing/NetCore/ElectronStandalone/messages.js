const UndefinedArgumentError = require('./undefinedArgumentError');
const fontawesome = require('@fortawesome/fontawesome');
const faExclamationTriangle = require('@fortawesome/fontawesome-free-solid/faExclamationTriangle');
fontawesome.library.add(faExclamationTriangle);
fontawesome.config.searchPseudoElements = true;

const classes = new Set([
    'error',
    'success',
    'warning'
]);
const _element = new WeakMap();
const _setClass = new WeakMap();

class Messages {
    constructor(element = UndefinedArgumentError.throwFor('element')) {
        _element.set(this, element);
        _setClass.set(this, className => {
            const classList = element.classList;
            [...classes].forEach(c => classList.remove(c));
            if (className) classList.add(className);
        });
    }

    clear() {
        _setClass.get(this)();
        _element.get(this).innerText = '';
    }

    update(message) {
        _element.get(this).innerText = message || '';
    }

    updateWithWarning(message) {
        _setClass.get(this)('warning');
        this.update(message);
    }

    updateWithSuccess(message) {
        _setClass.get(this)('success');
        this.update(message);
    }

    updateWithError(message) {
        _setClass.get(this)('error');
        this.update(message);
    }
}

module.exports = Messages;