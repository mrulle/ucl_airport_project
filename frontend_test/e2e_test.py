from selenium import webdriver
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.common.exceptions import TimeoutException
from selenium.webdriver.common.by import By
import time

print("Test Execution Started")
options = webdriver.ChromeOptions()
options.add_argument('--ignore-ssl-errors=yes')
options.add_argument('--ignore-certificate-errors')
driver = webdriver.Remote(
command_executor='http://localhost:4444/wd/hub',
options=options
)
#maximize the window size
driver.maximize_window()
time.sleep(10)
#navigate to browserstack.com
driver.get("http://frontend")
time.sleep(10)
#click on the Get started for free button
# driver.send_keys("")
try:
    WebDriverWait(driver, 3).until(EC.alert_is_present(),'Failed to fetch')

    alert = driver.switch_to.alert
    alert.accept()
    print("alert accepted")
except:
    print("no alert")
time.sleep(10)
nav_bar = driver.find_element(by=By.TAG_NAME, value="nav")
# this one only finds and returns the first element - og det duer jo bare ikke (generelt set)
links = nav_bar.find_element(by=By.TAG_NAME, value="a")
links.click()
print(links)
# check_in_button = driver.find_element(by=By.PARTIAL_LINK_TEXT, value="eck")
# check_in_button.click()
time.sleep(4)
#close the browser
driver.close()
driver.quit()
print("Test Execution Successfully Completed!")

