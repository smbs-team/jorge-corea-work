/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
importScripts('./sw-utils.js');
importScripts('./sw-handle-req.js');

// const mapTileServiceHost = new URL(location).searchParams.get(
//   'map-tile-service-host'
// );

/**
 * On sw install
 *@param e - The event
 */
self.addEventListener('install', (evt) => {
  evt.waitUntil(
    caches.open(STATIC_CACHE).then((cache) => cache.addAll(APP_SHELL))
  );
});

/**
 * On sw activate
 * @param event - The event
 */
self.addEventListener('activate', (event) => {
  event.waitUntil(
    caches.keys().then((keyList) => {
      return Promise.all(
        keyList.map((key) => {
          if ([STATIC_CACHE, DYNAMIC_CACHE].indexOf(key) === -1) {
            return caches.delete(key);
          }
        })
      );
    })
  );
});

/**
 * on fetch
 * @param event - the event
 */
self.addEventListener('fetch', (event) => {
  if (event.request.clone().method === 'GET') {
    return handleGetReq(event);
  }
  return event.respondWith(fetch(event.request));
});

addEventListener('backgroundfetchsuccess', (event) => {
  event.waitUntil(
    (async function () {
      try {
        self.registration.showNotification('File downloaded!');
        const cache = await caches.open(event.registration.id);
        const records = await event.registration.matchAll();
        const promises = records.map(async (record) => {
          const response = await record.responseReady;
          await cache.put(record.request, response);
        });
        await Promise.all(promises);
      } catch (err) {
        event.updateUI({ title: `Download failed` });
      }
    })()
  );
});

addEventListener('backgroundfetchclick', async (event) => {
  console.log('[Service Worker]: Background Fetch Click', event.registration);
  const records = await event.registration.matchAll();
  console.log(records);
});

addEventListener('backgroundfetchabort', (event) => {
  console.log('[Service Worker]: Background Fetch Abort', event.registration);
  console.error('Aborted by the user. No data was saved.');
});

addEventListener('backgroundfetchfail', (event) => {
  console.log('[Service Worker]: Background Fetch Fail', event.registration);
  event.waitUntil(
    (async function () {
      try {
        const cache = await caches.open(event.registration.id);
        const records = await event.registration.matchAll();
        const promises = records.map(async (record) => {
          const response = await record.responseReady;
          if (response && response.ok) {
            await cache.put(record.request, response);
          }
        });
        await Promise.all(promises);
      } finally {
        // Updating UI
        await event.updateUI({
          title: `${event.registration.id} failed: ${event.registration.failureReason}`,
        });
      }
    })()
  );
});

addEventListener('message', (event) => {
  console.log('A message received in service worker');
  console.log(event);
});
