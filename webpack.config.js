var path = require('path');
var Extracttextplugin = require('extract-text-webpack-plugin');
var ExtractPlugin = new Extracttextplugin({
    filename: 'main.css'
});
var HtmlWebPackPlugin = require('html-webpack-plugin');
var CleanWebPackPlugin = require('clean-webpack-plugin');
var webpack = require('webpack');


//var $ = 
/*
            {
                test: /\.scss$/,
                use: ExtractPlugin.extract({
                    use: [
                        'css-loader', 'sass-loader', 'style-loader',  'postcss-loader'{
                            loader: 'postcss-loader',
                            options: {
                                plugins: function () {
                                    return [ require('autoprefixer')]
                                }
                            }
                        }
                    ]
                })
            },
*/

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
                  loader: 'style-loader', // inject CSS to page
                }, {
                  loader: 'css-loader', // translates CSS into CommonJS modules
                }, {
                  loader: 'postcss-loader', // Run post css actions
                  options: {
                    plugins: function () { // post css plugins, can be exported to postcss.config.js
                      return [
                        require('precss'),
                        require('autoprefixer')
                      ];
                    }
                  }
                }, {
                  loader: 'sass-loader' // compiles Sass to CSS
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