const { Builder, By, Key, until, WebDriver } = require('selenium-webdriver');

// Configure the WebDriver
const driver = new Builder()
  .forBrowser('firefox')
  .usingServer('http://localhost:4444/wd/hub') // Selenium server address
  .build();

// Test logic
(async function example() {
  try {
    await driver.get('http://frontend');

    let title = await driver.getTitle();

    console.log(title);

    // Perform more actions or assertions
    // ...

    // // Example: Capture a screenshot for debugging purposes
    // await driver.takeScreenshot().then(
    //   function(image) {
    //     require('fs').writeFileSync('screenshot.png', image, 'base64');
    //   }
    // );
  } catch (error) {
    console.error('Test error:', error);
  } finally {
    // Close the browser session
    await driver.quit();
  }
})();