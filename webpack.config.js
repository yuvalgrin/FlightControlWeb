const path = require("path");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const CleanWebpackPlugin = require("clean-webpack-plugin");

const settings = {
    distPath: path.join(__dirname, "./../wwwroot"),
    srcPath: path.join(__dirname, "src")
};

function srcPathExtend(subpath) {
    return path.join(settings.srcPath, subpath)
}

module.exports = {
    entry: './src/index.js',
    output: {
        path: __dirname + './../wwwroot',
        publicPath: '/',
        filename: 'bundle.js'
    },
    devServer: {
        contentBase: '../wwwroot',
        writeToDisk: true
    },
    module: {
        rules: [
            {
                test: /\.(js|jsx)$/,
                exclude: /node_modules/,
                use: ['babel-loader']
            },
            {
                test: /\.(jpe?g|png|gif|svg|ico)$/i,
                use: [
                    {
                        loader: "file-loader",
                        options: {
                            outputPath: "assets/"
                        }
                    }
                ]
            },
            {
                test: /\.css$/i,
                use: ['style-loader', 'css-loader'],
            },
        ]
    },
    plugins: [
        new CleanWebpackPlugin([settings.distPath], {
            verbose: true
        }),
        new HtmlWebpackPlugin({
            template: srcPathExtend("index.html")
        })
    ],
};