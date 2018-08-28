var path = require('path');
var extracttestplugin = require('extract-text-webpack-plugin');
//var extracttestplugin = require('mini-css-extract-plugin');
var extractPlugin = new extracttestplugin({
    filename: 'main.css'
})

module.exports = {
    entry: './src/js/app.js',
    output: {
        path: path.resolve(__dirname, 'dist'),
        filename: 'bundle.js',
        publicPath: '/dist'
    },
    module: {
        rules: [
            {
                test: /\.js$/,
                use: [
                    {
                        loader: 'babel-loader',
                        options: {
                            presets: ['env']
                        }
                    }
                ]
            },
            {
                test: /\.scss$/,
                use: extractPlugin.extract({
                    use: ['css-loader', 'scss-loader']
                })
            }
        ]
    },
    plugins: [
        extractPlugin
    ]
}