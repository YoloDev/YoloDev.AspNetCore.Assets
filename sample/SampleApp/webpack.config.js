require('babel-register');
module.exports = [
    require('./config/webpack.dev').default,
    require('./config/webpack.prod').default
];
