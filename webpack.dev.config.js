const merge = require('webpack-merge');
const baseConfig = require('./webpack.config.js');
const DotEnv = require('dotenv-webpack');

module.exports = merge(baseConfig, {
    mode: 'production',
    plugins: [
        new DotEnv({
            path: './src/js/config/development.env'
        }),
    ]    
  });