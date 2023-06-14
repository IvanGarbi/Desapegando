(function (factory) {
    typeof define === 'function' && define.amd ? define(factory) :
        factory();
})((function () {
    'use strict';

    // import * as echarts from 'echarts';
    const { merge } = window._;

    // form config.js
    const echartSetOption = (
        chart,
        userOptions,
        getDefaultOptions,
        responsiveOptions
    ) => {
        const { breakpoints, resize } = window.phoenix.utils;
        const handleResize = options => {
            Object.keys(options).forEach(item => {
                if (window.innerWidth > breakpoints[item]) {
                    chart.setOption(options[item]);
                }
            });
        };

        const themeController = document.body;
        // Merge user options with lodash
        chart.setOption(merge(getDefaultOptions(), userOptions));

        resize(() => {
            chart.resize();
            if (responsiveOptions) {
                handleResize(responsiveOptions);
            }
        });
        if (responsiveOptions) {
            handleResize(responsiveOptions);
        }

        themeController.addEventListener(
            'clickControl',
            ({ detail: { control } }) => {
                if (control === 'phoenixTheme') {
                    chart.setOption(window._.merge(getDefaultOptions(), userOptions));
                }
            }
        );
    };
    // -------------------end config.js--------------------

    const resizeEcharts = () => {
        const $echarts = document.querySelectorAll('[data-echart-responsive]');

        if ($echarts.length > 0) {
            $echarts.forEach(item => {
                const echartInstance = echarts.getInstanceByDom(item);
                echartInstance?.resize();
            });
        }
    };

    const navbarVerticalToggle = document.querySelector('.navbar-vertical-toggle');
    navbarVerticalToggle &&
        navbarVerticalToggle.addEventListener('navbar.vertical.toggle', e => {
            return resizeEcharts();
        });

    const echartTabs = document.querySelectorAll('[data-tab-has-echarts]');
    echartTabs &&
        echartTabs.forEach(tab => {
            tab.addEventListener('shown.bs.tab', e => {
                const el = e.target;
                const { hash } = el;
                const id = hash ? hash : el.dataset.bsTarget;
                const content = document.getElementById(id.substring(1));
                const chart = content?.querySelector('[data-echart-tab]');
                chart && window.echarts.init(chart).resize();
            });
        });

    const tooltipFormatter = (params, dateFormatter = 'MMM DD') => {
        let tooltipItem = ``;
        params.forEach(el => {
            tooltipItem += `<div class='ms-1'>
                <h6 class="text-700"><span class="fas fa-circle me-1 fs--2" style="color:${el.borderColor ? el.borderColor : el.color
                }"></span>
                  ${el.seriesName} : ${typeof el.value === 'object' ? el.value[1] : el.value
                }
                </h6>
              </div>`;
        });
        return `<div>
                    <p class='mb-2 text-600'>
                      ${window.dayjs(params[0].axisValue).isValid()
                ? window.dayjs(params[0].axisValue).format(dateFormatter)
                : params[0].axisValue
            }
                    </p>
                    ${tooltipItem}
                  </div>`;
    };

    const basicLineChartInit = (teste) => {
        const { getColor, getData } = window.phoenix.utils;
        const $chartEl = document.querySelector('.echart-line-chart-example');
        const months = [
            'January',
            'February',
            'March',
            'April',
            'May',
            'June',
            'July',
            'August',
            'September',
            'October',
            'November',
            'December'
        ];

        const data = [
            1000, 1500, teste[0], teste[1], teste[2], 2000, 1200, 1330, 1000, 1200, 1410, 1200
                ];

    const tooltipFormatter = params => {
        return `
            <div>
                <h6 class="fs--1 text-700 mb-0">
                  <span class="fas fa-circle me-1" style='color:${params[0].borderColor}'></span>
                  ${params[0].name} : ${params[0].value}
                </h6>
            </div>
            `;
    };

    if ($chartEl) {
        const userOptions = getData($chartEl, 'echarts');
        const chart = window.echarts.init($chartEl);
        const getDefaultOptions = () => ({
            tooltip: {
                trigger: 'axis',
                padding: [7, 10],
                backgroundColor: getColor('gray-100'),
                borderColor: getColor('gray-300'),
                textStyle: { color: getColor('dark') },
                borderWidth: 1,
                transitionDuration: 0,
                formatter: tooltipFormatter,
                axisPointer: {
                    type: 'none'
                }
            },
            xAxis: {
                type: 'category',
                data: months,
                boundaryGap: false,
                axisLine: {
                    lineStyle: {
                        color: getColor('gray-300')
                    }
                },
                axisTick: { show: false },
                axisLabel: {
                    color: getColor('gray-400'),
                    formatter: value => value.substring(0, 3),
                    margin: 15
                },
                splitLine: {
                    show: false
                }
            },
            yAxis: {
                type: 'value',
                splitLine: {
                    lineStyle: {
                        type: 'dashed',
                        color: getColor('gray-200')
                    }
                },
                boundaryGap: false,
                axisLabel: {
                    show: true,
                    color: getColor('gray-400'),
                    margin: 15
                },
                axisTick: { show: false },
                axisLine: { show: false },
                min: 600
            },
            series: [
                {
                    type: 'line',
                    data,
                    itemStyle: {
                        color: getColor('white'),
                        borderColor: getColor('primary'),
                        borderWidth: 2
                    },
                    lineStyle: {
                        color: getColor('primary')
                    },
                    showSymbol: false,
                    symbol: 'circle',
                    symbolSize: 10,
                    smooth: false,
                    hoverAnimation: true
                }
            ],
            grid: { right: '3%', left: '10%', bottom: '10%', top: '5%' }
        });
        echartSetOption(chart, userOptions, getDefaultOptions);
    }
};


const { docReady } = window.phoenix.utils;

docReady(basicLineChartInit(dados));


        }));
        //# sourceMappingURL=echarts-example.js.map