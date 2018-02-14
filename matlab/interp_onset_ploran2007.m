clear all;

inFile = '_comparator_rois_tcs_for_interp.xls';
inSheet = {'r1_thal', 'r2_caudate', 'r3_presma'};
outFile = 'output_comparator_onsets_onsetThresh0.10.txt';
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
% threshold for onset (peak magnitude times this percentage will be called
% onset)
fonsetThresh = 0.2;

%~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%
tp = 1:nTp;
tp_i = 1/nTp_i;
nx_i = (nTp * nTp_i) - (nTp_i - 1);
min_onset = (nOnsetStartMin * nTp_i);
max_onset = (nOnsetStartMax * nTp_i) - (nTp_i - 1);

nRegions = length(inSheet);
nSubj = nSubj;
nRows = nEvents * nSubj;

output(1:nRows, 1:nRegions) = 0;

for i = 1:nRegions
    
    clear onsetMag onsetTime baseline;
    onsetTime(nRows) = 0; onsetTime = onsetTime';
    onsetMag(nRows) = 0; onsetMag = onsetMag';
    y_i(nRows,nx_i) = 0;
    x_i = 1:tp_i:nTp;
    
    [input] = xlsread(inFile,inSheet{i});
    nCol = size(input,2);
    
    tc(:,tp)=input(:,3:nCol);
    y_i(:,:) = interp1(tc(:,:)',x_i,'linear')';
    
    for row = 1:nRows
        [peakMag, peakTime] = max(tc(row,:));
        thresh = peakMag * fonsetThresh;
        x_i = (peakTime * nTp_i) -(nTp_i - 1);
        while x_i ~= 0 && x_i > (min_onset - nStep) && y_i(row,x_i) > thresh;
            x_i = x_i - nStep;
        end
        onsetMag(row)=mean(y_i(row,x_i));
        onsetTime(row)=((x_i+999)/1000);
    end 
    output(:,i) = onsetTime(:);
end
save (outFile, 'output', '-ASCII')
output
outFile