const _rectangle = new WeakMap();
const _info = new WeakMap();
const _canvas = new WeakMap();
const _context = new WeakMap();
const _strokeStyle = new WeakMap();
const _drag = new WeakMap();
const _reset = new WeakMap();
const _mouseDown = new WeakMap();
const _mouseMove = new WeakMap();
const _mouseUp = new WeakMap();
const _draw = new WeakMap();
const _setInfoText = new WeakMap();

class Marquee {
    constructor(canvas, infoElement, colour = '#fff') {
        if (canvas === undefined) throw new Error('Undefined argument (canvas).');

        _rectangle.set(this, {});
        _info.set(this, infoElement);
        _canvas.set(this, canvas);
        _context.set(this, canvas.getContext('2d'));
        _strokeStyle.set(this, colour);
        _drag.set(this, false);

        _setInfoText.set(this, string => (_info.get(this) || {}).innerText = string);

        _reset.set(this, () => {
            const canvas = _canvas.get(this);
            _context.get(this).clearRect(0, 0, canvas.width, canvas.height);

            _setInfoText.get(this)('');

            const rectangle = _rectangle.get(this);
            rectangle.x = 0;
            rectangle.y = 0;
            rectangle.width = canvas.width;
            rectangle.height = canvas.height;
        });

        _mouseDown.set(this, e => {
            this.reset();
            const rectangle = _rectangle.get(this);
            rectangle.x = e.offsetX;
            rectangle.y = e.offsetY;
            _drag.set(this, true);
        });

        _mouseMove.set(this, e => {
            if (_drag.get(this)) {
                const rectangle = _rectangle.get(this);
                rectangle.width = e.offsetX - rectangle.x;
                rectangle.height = e.offsetY - rectangle.y;
                _draw.get(this)();

                _setInfoText.get(this)(`x: ${this.x} y: ${this.y}  w: ${this.width} h: ${this.height}`);
            }
        });

        _mouseUp.set(this, () => {
            _drag.set(this, false);

            const canvas = _canvas.get(this);
            if (this.width === canvas.width && this.height === canvas.height) this.reset();
        });

        _draw.set(this, () => {
            const canvas = _canvas.get(this);
            const context = _context.get(this);
            context.clearRect(0, 0, canvas.width, canvas.height);
            context.beginPath();
            context.strokeStyle = _strokeStyle.get(this);
            context.rect(this.x, this.y, this.width, this.height);
            context.stroke();
        });

        canvas.onmousedown = _mouseDown.get(this);
        canvas.onmouseup = _mouseUp.get(this);
        canvas.onmousemove = _mouseMove.get(this);
        this.reset();
    }

    get x() {
        return _rectangle.get(this).x;
    }

    get y() {
        return _rectangle.get(this).y;
    }

    get width() {
        return _rectangle.get(this).width;
    }

    get height() {
        return _rectangle.get(this).height;
    }

    reset() {
        _reset.get(this)();
    }
};

module.exports = Marquee;