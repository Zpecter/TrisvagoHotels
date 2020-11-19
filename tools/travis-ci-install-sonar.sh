#!/bin/sh
echo "Starting install..."
wget -O sonar.zip https://github.com/SonarSource/sonar-scanner-msbuild/releases/download/5.0.4.24009/sonar-scanner-msbuild-5.0.4.24009-net5.0.zip
echo "Unzipping..."
unzip -qq sonar.zip -d tools/sonar
echo "Changing permissions..."
chmod +x tools/sonar/sonar-scanner-msbuild-5.0.4.24009-net5.0/sonar-scanner-4.4.0.2170/bin/sonar-scanner
