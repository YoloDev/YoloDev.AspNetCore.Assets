import { resolve } from 'path';
import webpack from 'webpack';

export default {
    entry: {
        'main': './assets/main'
    },

    output: {
        filename: '[name].js',
        path: resolve(__dirname, '..', 'wwwroot', 'assets')
    },

    module: {
        rules: [
            { test: /\.ts$/, use: 'ts-loader' }
        ]
    },

    resolve: {
        extensions: ['.ts', '.js']
    },

    plugins: [
        new webpack.optimize.CommonsChunkPlugin({
            name: 'vendor',
            minChunks: module => module.context && module.context.indexOf('node_modules') !== -1,
        })
    ]
};
