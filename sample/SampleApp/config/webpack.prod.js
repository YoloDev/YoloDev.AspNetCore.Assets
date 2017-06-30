import webpack from 'webpack';
import ManifestPlugin from 'webpack-assets-manifest';
import merge from 'webpack-merge';

import common from './webpack.common';

export default merge(common, {
    devtool: 'module-source-map',

    output: {
        filename: '[name].[chunkhash].js',
    },

    plugins: [
        /**
         * Create a manifest that can be used to determine the correct files to
         * load at runtime.
         */
        new ManifestPlugin(),

        /**
         * Assign the module and chunk ids by occurrence count
         * Reduces total file size and is recommended
         */
        new webpack.optimize.OccurrenceOrderPlugin(),

        /**
         * Create global constants which can be configured at compile time.
         *
         * Useful for allowing different behaviour between development builds and
         * release builds
         *
         * NODE_ENV should be production so that modules do not perform certain
         * development checks
         */
        new webpack.DefinePlugin({
            'process.env.NODE_ENV': JSON.stringify('production')
        }),

        /**
         * Babli is an ES6+ aware minifier based on the Babel toolchain (beta)
         */
        new webpack.optimize.UglifyJsPlugin({
            sourceMap: true
        }),
        //new BabiliPlugin()
    ]
});
