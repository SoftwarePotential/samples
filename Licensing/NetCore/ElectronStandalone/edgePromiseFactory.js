const path = require('path');
const edge = require('electron-edge-js');
const edgeAppRoot = process.env.EDGE_APP_ROOT;

const _assembly = new WeakMap();
const _typeName = new WeakMap();

class EdgePromiseFactory {
    constructor(assembly, typeName) {
        _assembly.set(this, assembly);
        _typeName.set(this, typeName);
    }

    create(methodName) {
        const fn = edge.func({
            assemblyFile: path.join(edgeAppRoot, _assembly.get(this)),
            typeName: _typeName.get(this),
            methodName: methodName
        });

        return arg => new Promise((resolve, reject) => {
            fn(arg, function (err, result) {
                if (err) reject(err);
                else resolve(result);
            });
        });
    }
}

module.exports = EdgePromiseFactory;