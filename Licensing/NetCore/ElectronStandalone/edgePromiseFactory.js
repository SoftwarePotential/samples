const path = require('path');
const edgeAppRoot = path.join(__dirname, 'dotNetAssemblies');

process.env.EDGE_USE_CORECLR = 1;
process.env.EDGE_APP_ROOT = edgeAppRoot;
const edge = require('electron-edge-js');

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