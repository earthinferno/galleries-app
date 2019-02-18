const path = require('path');
const MiniCssExtractPlugin = require("mini-css-extract-plugin")
const HtmlWebPackPlugin = require('html-webpack-plugin');
const CleanWebPackPlugin = require('clean-webpack-plugin');
const webpack = require('webpack');
//const DotEnv = require('dotenv-webpack');

module.exports = {
    entry: ['babel-polyfill','./src/js/index.js'],
    output: {
        path: path.resolve(__dirname, 'dist'),
        filename: 'bundle.js',
        // publicPath: '/dist'
    },
    module: {
        rules: [
            {
                test: /\.jsx?$/,
                exclude: /node_modules/,
                use: [
                    {
                        loader: 'babel-loader',
                        options: {
                            presets: ['env','react']
                        }
                    }
                ]
            },
            {
                test: /\.(scss)$/,
                use: [{
                  // Adds CSS to the DOM by injecting a `<style>` tag
                  loader: 'style-loader',
                }, {
                    loader: MiniCssExtractPlugin.loader,
                }, {
                  // Interprets `@import` and `url()` like `import/require()` and will resolve them
                  loader: 'css-loader', 
                }, {
                  // Loads a SASS/SCSS file and compiles it to CSS
                  loader: 'sass-loader' 
                }]                
            },
            {
                test: /\.html$/,
                use: ['html-loader']
            },
            {
                test: /\.(jpg|png)/,
                use: [
                    {
                        loader: 'file-loader',
                        options: {
                            name: '[name].[ext]',
                            outputPath: 'images/',
                            publicPath: 'images/'
                        }
                    }
                ]
            },
            {
                test:  /\.(config)$/,
                use: [
                    {
                        loader: 'file-loader',
                        options: {
                            name: '[name].[ext]'                        }
                    }
                ]
            }            
        ]
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: "style.css"
          }),
        new HtmlWebPackPlugin({
            template: './src/index.html'
        }),
        new CleanWebPackPlugin(['dist']),
        new webpack.ProvidePlugin({
            '$':'jquery',
        }),
    ]
}