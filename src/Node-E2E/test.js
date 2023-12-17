const { Builder, By, Key, until, WebDriver } = require('selenium-webdriver');
const assert = require('assert');

let driver;

describe('End-to-End Tests', function() {

  it('Should load the homepage', async function() {
    try {
      driver = new Builder()
        .forBrowser('firefox')
        .usingServer('http://localhost:4444/wd/hub') // Selenium server address
        .build()

      await driver.get('http://frontend');

      const title = await driver.getTitle();

      assert.strictEqual(title, 'Vite App');
      
    } catch (error) {
      console.log('Test error:', error);
    } finally {
      driver.quit();
    }
  });
});

