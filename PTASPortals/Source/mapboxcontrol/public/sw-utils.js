/**
 * Cache version
 */
const cacheVersion = '0.2.';

const isLocalhost = Boolean(
  location.hostname === 'localhost' ||
    // [::1] is the IPv6 localhost address.
    location.hostname === '[::1]' ||
    // 127.0.0.0/8 are considered localhost for IPv4.
    location.hostname.match(
      /^127(?:\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}$/
    )
);

/**
 * Static cache key
 */
const DYNAMIC_CACHE = `dynamic-${cacheVersion}`;
const STATIC_CACHE = `static-${cacheVersion}`;

/**
 * Initial files
 */
const APP_SHELL = [
  'https://fonts.googleapis.com/css2?family=Cabin:wght@400;600;700&family=Roboto+Condensed&display=swap',
];
