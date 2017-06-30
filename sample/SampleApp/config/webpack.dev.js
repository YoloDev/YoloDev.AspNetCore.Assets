import webpack from 'webpack';
import merge from 'webpack-merge';

import common from './webpack.common';

export default merge(common, {
    devtool: 'inline-source-map',

    plugins: [
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
            'process.env.NODE_ENV': JSON.stringify('development')
        }),
    ]
});
