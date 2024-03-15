const Nightmare = require('nightmare');
const assert = require('assert');
const fs = require('fs');
const path = require('path');
const test_url = "file:///"+path.resolve(__dirname,"../examples/vanilla.html");
const hash_url = test_url + "#group=nogroup&photo=rhino";

const nightmare = Nightmare({
    webPreferences  : {
    partition : 'nopersist',
    preload: path.resolve(__dirname,'./preload.js')
  },
  show: true
});

describe('test',() => {
  it('caption', (done) => {
    nightmare.goto(test_url)
      .click('[data-caption="Lion"]')
      .wait(1000)
      .click('.smartphoto-arrow-right a')
      .wait(1000)
      .evaluate(() => {
        return document.querySelector('.smartphoto-caption').innerText;
      })
      .then((result) => {
        assert.equal(result,'Camel');
        done();
      })
      .catch((error) => {
        done(error);
      });
  });

  it('hash', (done) => {
    nightmare.goto(hash_url)
      .wait(1000)
      .evaluate(() => {
          return document.querySelector('.smartphoto-caption').innerText;
      })
      .then(result => {
        assert.equal(result,'Rhino');
        done();
      })
      .catch(error => {
        done(error);
      });
  });
});
