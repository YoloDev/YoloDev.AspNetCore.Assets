require('babel-register');
/* module.exports = [
    require('./config/webpack.dev').default,
    require('./config/webpack.prod').default
]; */

module.exports = env =>
  env === 'production'
    ? require('./config/webpack.prod').default
    : require('./config/webpack.dev').default;
