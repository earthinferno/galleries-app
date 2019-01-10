var path = require('path');
var Extracttextplugin = require('extract-text-webpack-plugin');
var ExtractPlugin = new Extracttextplugin({
    filename: 'main.css'
});
var HtmlWebPackPlugin = require('html-webpack-plugin');
var CleanWebPackPlugin = require('clean-webpack-plugin');
var webpack = require('webpack');


//var $ = 

module.exports = {
    entry: './src/js/index.js',
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
                use: ['babel-loader','eslint-loader']
            },            
            {
                test: /\.jsx?$/,
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
                test: /\.scss$/,
                use: ExtractPlugin.extract({
                    use: ['css-loader', 'sass-loader']
                })
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
        ExtractPlugin,
        new HtmlWebPackPlugin({
            template: './src/index.html'
        }),
        new CleanWebPackPlugin(['dist']),
        new webpack.ProvidePlugin({
            '$':'jquery',
        }),
    ]
}