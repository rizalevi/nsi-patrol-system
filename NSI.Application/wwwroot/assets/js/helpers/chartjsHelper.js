function getDynamicScale(maxValue) {

    const exponent = Math.floor(Math.log10(maxValue));
    const fraction = maxValue / Math.pow(10, exponent);

    let niceFraction;

    if (fraction <= 1) niceFraction = 1;
    else if (fraction <= 2) niceFraction = 2;
    else if (fraction <= 5) niceFraction = 5;
    else niceFraction = 10;

    const niceMax = niceFraction * Math.pow(10, exponent);

    return niceMax;
}

function getAutoGridCount(maxValue) {
    if (maxValue <= 10) return 4;
    if (maxValue <= 50) return 5;
    if (maxValue <= 100) return 6;
    if (maxValue <= 500) return 6;
    if (maxValue <= 1000) return 8;
    return 10;
}