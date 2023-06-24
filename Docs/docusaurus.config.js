// @ts-check
// Note: type annotations allow type checking and IDEs autocompletion

const lightCodeTheme = require('prism-react-renderer/themes/github');
const darkCodeTheme = require('prism-react-renderer/themes/dracula');

/** @type {import('@docusaurus/types').Config} */
const config = {
  title: 'Small Steamworks',
  tagline: 'Steamworks made easy',
  favicon: 'img/favicon.png',

  // Set the production url of your site here
  url: 'https://hertzole.se',
  // Set the /<baseUrl>/ pathname under which your site is served
  // For GitHub pages deployment, it is often '/<projectName>/'
  baseUrl: '/small-steamworks/',

  // GitHub pages deployment config.
  // If you aren't using GitHub pages, you don't need these.
  organizationName: 'Hertzole', // Usually your GitHub org/user name.
  projectName: 'small-steamworks', // Usually your repo name.

  onBrokenLinks: 'throw',
  onBrokenMarkdownLinks: 'throw',

  // Even if you don't use internalization, you can use this field to set useful
  // metadata like html lang. For example, if your site is Chinese, you may want
  // to replace "en" with "zh-Hans".
  i18n: {
    defaultLocale: 'en',
    locales: ['en'],
  },

  plugins: [require.resolve("@cmfcmf/docusaurus-search-local")],

  presets: [
    [
      'classic',
      /** @type {import('@docusaurus/preset-classic').Options} */
      ({
        docs: {
          routeBasePath: '/',
          sidebarPath: require.resolve('./sidebars.js'),
        },
        theme: {
          customCss: require.resolve('./src/css/custom.css'),
        },
      }),
    ],
  ],

  themeConfig:
    /** @type {import('@docusaurus/preset-classic').ThemeConfig} */
    ({
      // Replace with your project's social card
      navbar: {
        title: 'Small Steamworks',
        items: [
          {
            type: 'doc',
            sidebarId: 'tutorialSidebar',
            position: 'left',
            docId: 'intro',
            label: 'Documentation',
          },
        ],
      },
      footer: {
        style: 'dark',
        links: [
          {
            title: 'Links',
            items: [
              {
                label: 'GitHub Repository',
                href: 'https://github.com/hertzole/small-steamworks',
              },
              {
                label: 'Steamworks.NET',
                href: 'https://github.com/rlabrecque/Steamworks.NET',
              },
              {
                label: 'Steamworks Documentation',
                href: 'https://partner.steamgames.com/doc/home'
              }
            ],
          },
          {
            title: 'More',
            items: [
              {
                label: 'hertzole.se',
                href: 'https://hertzole.se',
              },
              {
                label: 'Hertzole GitHub',
                href: 'https://github.com/hertzole',
              }
            ],
          },
        ],
        copyright: `Copyright © ${new Date().getFullYear()} Small Steamworks, Hertzole. Built with Docusaurus.`,
      },
      prism: {
        theme: lightCodeTheme,
        darkTheme: darkCodeTheme,
        additionalLanguages: ['csharp', 'json']
      },
    }),
};

module.exports = config;
