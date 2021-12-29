create_app_structure() {
    APPNAME=$1
    APPDIR="$APPNAME.app/Contents"
    APPICONS="/System/Library/CoreServices/CoreTypes.bundle/Contents/Resources/GenericApplicationIcon.icns"

    if [ ! -d "$APPDIR" ]; then
        echo "creating app structure $APPDIR"

        mkdir -vp "$APPDIR"/{Resources,Frameworks}
        cp -v "$APPICONS" "$APPDIR/Resources/$APPNAME.icns"
    fi
}

emit_plist() {
    PLIST_APPNAME=$1
    PLIST_PATH="$2/Info.plist"
    
    if [ "$3" ]; then
        LSUIELEMENT="false"
    else
        LSUIELEMENT="true"
    fi
    
    if [ ! -f "$PLIST_PATH" ]; then
        echo "emiting $PLIST_PATH"
        cat <<EOF > "$PLIST_PATH"
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>CFBundleExecutable</key>
    <string>$PLIST_APPNAME</string>
    <key>CFBundleGetInfoString</key>
    <string>$PLIST_APPNAME</string>
    <key>CFBundleIconFile</key>
    <string>$PLIST_APPNAME</string>
    <key>CFBundleName</key>
    <string>$PLIST_APPNAME</string>
    <key>CFBundlePackageType</key>
    <string>APPL</string>
    <key>CFBundleSignature</key>
    <string>????</string>
    <key>LSUIElement</key>
    <string>$LSUIELEMENT</string>
</dict>
</plist>
EOF
    fi
}


BINARYNAME="AvaloniaApp"
CONFIGURATION="Debug"
NETTARGET="netcoreapp3.1"
CEFZIP="cef_binary_97.0.8%2Bg4eb2da6%2Bchromium-97.0.4692.36_macosx64_beta_minimal.tar.bz2"
TARGET="bin"


if [ ! -d "$TARGET" ]; then
    mkdir "$TARGET"
fi

cd "$TARGET"

echo "Building CefNet Avalonia demo..."
dotnet publish ../AvaloniaApp/AvaloniaApp.csproj -c "$CONFIGURATION" -r osx-x64 -f "$NETTARGET" -o "publish" --self-contained

CEFBINARIES="../cef"
CEFFRAMEWORK_DIR="$(find $CEFBINARIES -name "Release")"

if [ ! -d "$CEFFRAMEWORK_DIR" ]; then
    if [ ! -f "$CEFBINARIES/$CEFZIP" ]; then
        echo "downloading cef binaries from https://cef-builds.spotifycdn.com/$CEFZIP"
        curl -o "$CEFBINARIES/$CEFZIP" "https://cef-builds.spotifycdn.com/$CEFZIP"
    fi
    echo "unzipping cef binaries"
    tar -jxvf "$CEFBINARIES/$CEFZIP" --strip-components 1 -C "./$CEFBINARIES"
    CEFFRAMEWORK_DIR="$(find $CEFBINARIES -name "Release")"
fi


APPNAME="$BINARYNAME"
APPDIR="$APPNAME.app/Contents"

rm -rf "$APPDIR"

create_app_structure "$APPNAME"
emit_plist "$APPNAME" "$APPDIR" true

cp -R "$CEFFRAMEWORK_DIR/Chromium Embedded Framework.framework" "$APPDIR/Frameworks/"

cd "$APPDIR/Frameworks"

APPNAME="$BINARYNAME Helper"
APPDIR="$APPNAME.app/Contents"
create_app_structure "$APPNAME"
emit_plist "$APPNAME" "$APPDIR"
cp -R "../../../publish/" "$APPDIR/MacOS"
ln -s "Frameworks/$APPDIR/MacOS" "../MacOS"
chmod +x "$APPDIR/MacOS/$BINARYNAME"
cp "$APPDIR/MacOS/$BINARYNAME" "$APPDIR/MacOS/$APPNAME"
chmod +x "$APPDIR/MacOS/$APPNAME"

APPNAME="$BINARYNAME Helper (GPU)"
APPDIR="$APPNAME.app/Contents"
mkdir -pv "$APPDIR"
emit_plist "$APPNAME" "$APPDIR"
ln -s "../../$BINARYNAME Helper.app/Contents/MacOS" "$APPDIR/MacOS"
cp "$APPDIR/MacOS/$BINARYNAME" "$APPDIR/MacOS/$APPNAME"
chmod +x "$APPDIR/MacOS/$APPNAME"

APPNAME="$BINARYNAME Helper (Plugin)"
APPDIR="$APPNAME.app/Contents"
mkdir -pv "$APPDIR"
emit_plist "$APPNAME" "$APPDIR"
ln -s "../../$BINARYNAME Helper.app/Contents/MacOS" "$APPDIR/MacOS"
cp "$APPDIR/MacOS/$BINARYNAME" "$APPDIR/MacOS/$APPNAME"
chmod +x "$APPDIR/MacOS/$APPNAME"

APPNAME="$BINARYNAME Helper (Renderer)"
APPDIR="$APPNAME.app/Contents"
mkdir -pv "$APPDIR"
emit_plist "$APPNAME" "$APPDIR"
ln -s "../../$BINARYNAME Helper.app/Contents/MacOS" "$APPDIR/MacOS"
cp "$APPDIR/MacOS/$BINARYNAME" "$APPDIR/MacOS/$APPNAME"
chmod +x "$APPDIR/MacOS/$APPNAME"
