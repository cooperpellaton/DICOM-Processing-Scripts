% This script will generate all the FFT data for an image set necessary to make 
% movie (.avi) stimuli.
%
% Input: all files should be in the same folder, numbered consecutively and .bmp format 
% [e.g., 1.bmp, 2.bmp, etc.] (you can always change line 77 to accept other formats).
% Change appropriate user options below. Noise %s can be entered into array below as a 
% decimal followed by a semicolon (e.g., {.35; .40; .99}; ). Script will generate 
% $nframes frames, assuming a frame rate of 15 frames/second (so 90 frames, for example,
% will give you 6 s of video). 
%
% Output: script will save a .mat file containing all the data necessary to create an 
% .avi file. Another script should then be run (FFT_image_make_movies.m) to create the 
% videos; be sure to open the .mat file beforehand.
%
% NOTE: the .mat files generated from this are huge. E.g., processing a set of 85 images 
% at 90 frames will generate about 5-6 GB of data for each noise level specified. Check 
% your HDD space before you run.

% J. Tremel (tremeljosh@gmail.com), 2010: Release, University of Pittsburgh
%       March 2011 - Minor Revision
%       April 2011 - Added option for convolution method

%~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%
% User options:
%~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%
    % cd to dir to process
    cd /Users/Shared/Wheeler_FaceHouse/FvH_Stimuli/orig_imgs/

    % Number of images to process (considering filenames are numbered consecutively)
    nImg = 85;

    %  Noise percentages to introduce into images.
    %+ Script will loop and save a .mat file for each noise level specified.
    noises = {.55; .56; .57; .58};

    % Number of frames to make for each image.
        % e.g., @ 15 fps, 90 frames = 6 s of video
    nframes = 90;
    
    % Method for phase scrambling ('linear' scrambling or 'awgn' convolution)
    method='awgn';
    %   >> linear scrambling = uses linear combination of coherence*original_phase + 
    %       (1-coherence)*random_phase. Most common method in literature.
    %   >> AWGN convolution = convolves original phase matrix with a noise matrix
    %       generated using an additive white gaussian noise filter. Convolves the two
    %       components at a specified signal to noise ratio (calculated from noise %)

%~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%
% SCRIPT:
%~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%
    % number of first image
    firstImg = 1;

    % For debug... (Econ at 0 keeps some intermediate data.)
    econ = 1;

for x = 1:length(noises)
    nsp = noises{x};

    nhundred = nsp*100;

    % calculate SNR in dB from specified percentages for awgn filter
    sgp = 1 - nsp;          % Signal %%
    spn = sgp/nsp;          % Signal %%/Noise %%
    snr = 10*log10(spn);    % dB conversion (for AWGN ONLY)
                            %   (Note: dB measurement is using the physics convention of 
                            %   10*log10(), not 20*log10() used in digital imaging. This
                            %   means a ratio of 100:1 (Sig:Noise) is 20 dB, not 40 dB.)
    
    iter = 1;
    while iter <= nframes
        iImg = firstImg;
        disp(sprintf('iteration %i',iter))    % Print iteration # to screen

        while iImg <= nImg
            iImgNm = deblank(sprintf('%02d %i',iImg)); % fix img # for strings
            filenm = sprintf('%s.bmp', iImgNm); % get filename

            rawimg{iImg} = imread(filenm); % read image
            imglength = length(rawimg{iImg}); % check image dimensions
            padto = pow2(nextpow2(imglength)); % pad to next power of 2 (fft efficiency)

            %  Take forward 2D-FFT of img. Derive amplitude and phase.
            FT{iImg}{iter} = fft2(double(rawimg{iImg}), padto, padto);
            FT_ampli{iImg}{iter} = abs(FT{iImg}{iter});         % amplitude
            FT_phase{iImg}{iter} = angle(FT{iImg}{iter});       % phase

            % Power spectrum; uncomment if you ever need it:
%             FT_power{iImg}{iter} = FT{iImg}{iter}.*conj(FT{iImg}{iter})/padto; % power
            
            iImg = iImg + 1;
            clear filenm
        end

        if econ == 1 % free up some memory
            clear rawimg
            clear FT
            clear filenm
        else
        end

        % create mean amplitude array (normalize luminance, contrast, etc.)
        iImg = firstImg;
        while iImg <= nImg
            if iImg == 1    % initialize array on first iteration
                FT_ampli_mean = FT_ampli{iImg}{iter};
                iImg = iImg + 1;
            else
                FT_ampli_mean = FT_ampli_mean + FT_ampli{iImg}{iter};
                iImg = iImg + 1;
            end
        end
        FT_ampli_mean = FT_ampli_mean./nImg;

        % Make noise matrix and convolve w/ phase matrices
        iImg = firstImg;
        while iImg <= nImg
            
            % Additive white gaussian noise phase convolution:
            if strcmp(method,'awgn') == 1
                FT_phase_wnoise{iImg}{iter} = awgn(FT_phase{iImg}{iter},snr);
            
            % Linear random noise matrix phase scrambling:
            elseif strcmp(method,'linear') == 1
                FT_randphase = angle(fft2(rand(padto,padto)));    
                FT_phase_wnoise{iImg}{iter} = (sgp.*FT_phase{iImg}{iter})+(nsp.*FT_randphase);
                clear FT_randphase
            else
                disp('No ''method'' found... Check parameters')
                break
            end
            iImg = iImg + 1;
        end

        % Rebuild image
        iImg = firstImg;
        while iImg <= nImg
            % rebuild signal from components
            FT_recomb{iImg}{iter} = (FT_ampli_mean.*exp(1i*FT_phase_wnoise{iImg}{iter}));
            
            % invert invert fft back to image, remove imaginary round off error
            procImg{iImg}{iter} = real(ifft2(FT_recomb{iImg}{iter}));
            
            % cut off pad to original image size
            finImg{iImg}{iter} = procImg{iImg}{iter}(1:imglength,1:imglength);
            
            iImg = iImg + 1;
        end
        
        if econ == 1
            clear FT_recomb
            clear procImg
        else
        end

        iter = iter + 1;

    end
    
    % clear intermediate vars to save memory before .mat save
		clear FT
		clear FT_ampli
		clear FT_phase
		clear rawimg
		clear FT_ampli_mean
		clear FT_phase_wnoise

    savefile = sprintf('noise%s.mat',num2str(nhundred));
    save(savefile,'-v7.3');
    
    % clear the rest after .mat is saved, loop on next noise level.
        clear nsp
        clear subdir
        clear nhundred
        clear sgp
        clear spn
        clear snr
        clear iter
        clear iImg
        clear filenm
        clear imglength
        clear padto
        clear finImg
end