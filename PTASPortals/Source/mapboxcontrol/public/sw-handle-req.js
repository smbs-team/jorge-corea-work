/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

const isStaticUrl = (url) =>
  new URL(url).searchParams.get('is-static') === 'true' ||
  APP_SHELL.find((item) => url === item);

const getFromNetwork = (event) =>
  fetch(event.request).then((newRes) => {
    const cacheKey = isStaticUrl(event.request.url)
      ? STATIC_CACHE
      : DYNAMIC_CACHE;
    if ([500, 400, 404].every((item) => newRes.status !== item)) {
      caches.open(cacheKey).then((cache) => {
        cache.put(event.request, newRes.clone());
      });
    } else {
      caches.open(cacheKey).then((cache) => {
        cache.delete(event.request);
      });
    }
    return newRes.clone();
  });

const getFromCache = async (event) =>
  caches.match(event.request).then(async (r1) => {
    if (r1) {
      if (!isStaticUrl(event.request.url)) {
        setTimeout(() => {
          getFromNetwork(event);
        }, 8000);
      }
      return r1;
    } else {
      const r = await getFromNetwork(event);
      console.log('Resource ' + event.request.url + ' resolved from network');
      return r;
    }
  });

const handleGetReq = (event) => {
  const url = new URL(event.request.url);
  const useCache = url.searchParams.get('cache') === 'true';
  if (APP_SHELL.find((item) => event.request.url.includes(item)) || useCache) {
    return event.respondWith(getFromCache(event));
  }
  return event.respondWith(fetch(event.request));
};
