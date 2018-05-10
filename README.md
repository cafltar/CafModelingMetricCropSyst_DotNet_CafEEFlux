# Readme

## Purpose

Command line utility to automate downloading of EEFlux images from the [EEFlux App](https://eeflux-level1.appspot.com/).

## Instructions

CafEEFlux is a cross-platform command line interface that can be run on numerous flavors of linux, Windows, and Mac OS.  The application will download multiple image layers supported by EEFlux within a given date range at a given location and filter by cloudiness and tier level.  The current version only downloads images generated from Landsat8.  All options are specified on the command line.  Use the option "-h" (or "--help") for details.

From the EEFlux website (https://eeflux-level1.appspot.com/):

> "A reference for general equations for EEFlux, based on those of METRIC is available at: http://www.intechopen.com/books/evapotranspiration-remote-sensing-and-modeling/operational-remote-sensing-of-et-and-challenges which is an Intech book chapter compiled by Dr. Ayse Kilic (Irmak) of the Univ. Nebraska-Lincoln and associates at the University of Idaho and Desert Research Institute in 2012. An original reference for METRIC is Allen et al., 2007 published in the ASCE J. Irrigation and Drainage Engineering."

```console
> CafEEFlux -h
Usage: eeflux [options]

Options:
  -?|-h|--help  Show help information
  --lat         Latitude (decimal degrees) for image location
  --lon         Longitude (decimal degrees) for image location
  --startdate   Starting date to get images; in form of yyyyMMdd
  --enddate     Ending date to get images; in form of yyyyMMdd
  --cloudiness  Percent cloudiness value (0-100), images with value above specified value will be excluded from download
  --tier        Tier value threshold (1,2), images with values above specified value will be excluded from downloaded
  --writepath   Absolute or relative path to write the files to.  If not specified, images will be downloaded to current working directory
  --imagetypes  Comma separated list of image types to download. Quotes are required. [true_color, false_color_4, false_color_7, albedo, ndvi, dem, land_use, lst, etr24, eto24, etrf, etof, eta]
  -q|--quiet    Sets quiet mode, meaning no user interaction.  Default is true. [true, false]
```

The images will be downloaded into the current working directory, unless otherwise specified by the --writepath option.

### Example

The following will download NDVI and ETOF images that overlap the point (46.781021, -117.081407) between April 01, 2015 and Sept. 01, 2016.  Images that have cloud cover more than 20 percent and a quality level of T2 or RT will not be downloaded.

```console
> CafEEFlux --lat 46.781021 --lon -117.081407 --startdate 20150401 --enddate 20160901 --cloudiness 20 -tier 1 --imagetypes "ndvi, etof" -q false
```

## License

As a work of the United States government, this project is in the public domain within the United States.

Additionally, we waive copyright and related rights in the work worldwide through the CC0 1.0 Universal public domain dedication.