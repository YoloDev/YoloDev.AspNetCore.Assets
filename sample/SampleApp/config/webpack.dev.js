import common from './webpack.common';
import merge from 'webpack-merge';
import webpack from 'webpack';

export default merge(common, {
  mode: 'development',
  devtool: 'inline-source-map',
  devServer: {},

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
      'process.env.NODE_ENV': JSON.stringify('development'),
    }),
  ],
});
