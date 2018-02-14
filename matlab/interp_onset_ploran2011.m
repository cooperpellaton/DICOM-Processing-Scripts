clear all;

inFile = '_comparator_rois_tcs_for_interp.xls';
inSheet = {'r1_thal', 'r2_caudate', 'r3_presma'};
outFile = 'output_comparator_onsets_zeroBaseline_p.05.txt';
nEvents = 4;
nSubj = 16;
% number of time points
nTp = 11;
% number of points between time points to interpolate
nTp_i = 1000;
% number of time points to move when stepping through each time series
nStep = 3;
% minimum and maximum time point for signal onset (1 = first time point)
nOnsetStartMin = 1;
nOnsetStartMax = nTp;
% alpha of the t-test
fAlpha = 0.05;
% false will assume a baseline of 0, true will use a noise estimate taking
% the average of the first and last time points to test against.
bUseNoiseEstimate = false;

%~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%
tp = 1:nTp;
tp_i = 1/nTp_i;
nx_i = (nTp * nTp_i) - (nTp_i - 1);
min_onset = (nOnsetStartMin * nTp_i) - (nTp_i - 1);
max_onset = (nOnsetStartMax * nTp_i) - (nTp_i - 1);

nRegions = length(inSheet);
nSubj = nSubj;
nRows = nEvents * nSubj;

output(1:nRows, 1:nRegions) = 0;

for i = 1:nRegions
    
    clear onsetMag onsetTime baseline;
    onsetTime(nRows) = 0; onsetTime = onsetTime';
    onsetMag(nRows) = 0; onsetMag = onsetMag';
    baseline(nSubj) = 0; baseline = baseline';
    y_i(nRows,nx_i) = 0;
    x_i = 1:tp_i:nTp;
    
    [input] = xlsread(inFile,inSheet{i});
    nCol = size(input,2);
    
    tc(:,tp)=input(:,3:nCol);
    y_i(:,:) = interp1(tc(:,:)',x_i,'linear')';
    
    startRow = 1;
    for event = 1:nEvents
        if (bUseNoiseEstimate)
            baseline(1:nSubj) = mean( [ tc(startRow:(startRow+nSubj-1),1)'; tc(startRow:(startRow+nSubj-1),nTp)' ] )';
        else
            baseline(1:nSubj) = 0;
        end
        x_i = min_onset;
        while x_i ~= 0 && x_i < (max_onset-nStep) && ttest2(baseline(1:nSubj), y_i(startRow:(startRow+nSubj-1),x_i), fAlpha) == 0;
            x_i = x_i + nStep;
        end
        onsetMag(startRow:(startRow+nSubj-1)) = mean(y_i(startRow:(startRow+nSubj-1),x_i));
        onsetTime(startRow:(startRow+nSubj-1)) = ((x_i+999)/1000);
        startRow = startRow + nSubj;
    end 
    output(:,i) = onsetTime;
end
save (outFile, 'output', '-ASCII')
output
outFile