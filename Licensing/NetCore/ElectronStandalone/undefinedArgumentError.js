class UndefinedArgumentError extends Error {
    constructor(parameter) {
        super(`Undefined Argument${parameter ? ` (${parameter})` : ''}.`);
    }

    static throwFor(parameter) {
        throw new UndefinedArgumentError(parameter);
    }
}

module.exports = UndefinedArgumentError;