// db.js
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

module.exports = () => {
  return {
    posts: [{ id: 1, title: 'json-server', author: 'typicode' }],
    comments: [{ id: 1, body: 'some comment', postId: 1 }],
    profile: { name: 'typicode' },
  };
};
