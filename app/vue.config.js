module.exports = {
    css: {
        loaderOptions: {
            sass: {
                prependData: `@import "@/assets/styles/theme.sass";`
            }
        }
    },
    configureWebpack: {
        devtool: 'source-map'
      }
};
