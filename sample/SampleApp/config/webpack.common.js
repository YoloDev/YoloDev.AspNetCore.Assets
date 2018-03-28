import { resolve } from 'path';
import webpack from 'webpack';

export default {
  entry: {
    main: './assets/main',
  },

  output: {
    filename: '[name].js',
    path: resolve(__dirname, '..', 'wwwroot', 'assets'),
    publicPath: '/assets/',
  },

  module: {
    rules: [{ test: /\.ts$/, use: 'ts-loader' }],
  },

  resolve: {
    extensions: ['.ts', '.js'],
  },
};
