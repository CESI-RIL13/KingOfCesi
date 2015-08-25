$(document).ready(function () {



    Handlebars.registerHelper('ifCond', function (v1, operator, v2, options) {
    switch (operator) {
        case '==':
            console.log(v1 + " == " + v2);
            return (v1 == v2) ? options.fn(this) : options.inverse(this);
        case '===':
            return (v1 === v2) ? options.fn(this) : options.inverse(this);
        case '!==':
            return (v1 !== v2) ? options.fn(this) : options.inverse(this);
        case '<':
            return (v1 < v2) ? options.fn(this) : options.inverse(this);
        case '<=':
            return (v1 <= v2) ? options.fn(this) : options.inverse(this);
        case '>':
            return (v1 > v2) ? options.fn(this) : options.inverse(this);
        case '>=':
            return (v1 >= v2) ? options.fn(this) : options.inverse(this);
        case '&&':
            return (v1 && v2) ? options.fn(this) : options.inverse(this);
        case '||':
            return (v1 || v2) ? options.fn(this) : options.inverse(this);
        default:
            return options.inverse(this);
    }
    });

    Handlebars.registerHelper('getDice', function (valeur) {
        switch (valeur) {
            case 0:
                return "dice--1";
            case 1:
                return "dice--2";
            case 2:
                return "dice--3";
            case 3:
                return "dice--attack";
            case 4:
                return "dice--energy";
            case 5:
                return "dice--hp";
        }
    });
});