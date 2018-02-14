clear all;

%define file names of region time courses (rows are time points 1:16 and columns are conditions
%divided into subjects [i.e., S1Cond1 S2Cond1 S3Cond1 ... etc. would be the column headers]);
% infile = {'ROI01_-27_-92_-02.xls'; 'ROI02_26_-91_05.xls'; 'ROI03_29_-89_-05.xls'; 'ROI04_17_-93_-06.xls';...
% 'ROI05_-26_-87_-13.xls'; 'ROI06_-17_-96_-10.xls'; 'ROI07_45_-64_-07.xls'; 'ROI08_-39_-81_-11.xls';...
% 'ROI09_42_-75_-07.xls'; 'ROI10_34_-82_18.xls'; 'ROI11_29_-73_30.xls'; 'ROI12_26_-60_55.xls';...
% 'ROI13_-31_-81_15.xls'; 'ROI14_-41_-63_-13.xls'; 'ROI15_24_-68_47.xls'; 'ROI16_31_-49_49.xls'; 'ROI17_-28_-70_-14.xls';...
% 'ROI18_-25_-54_47.xls'; 'ROI19_39_-39_48.xls'; 'ROI21_-27_-72_29.xls'; 'ROI22_-08_18_46.xls';...
% 'ROI23_28_-69_-10.xls'; 'ROI24_-30_-53_-17.xls'; 'ROI25_29_22_03.xls'; 'ROI26_27_-46_-16.xls'; 'ROI27_32_-57_-14.xls';...
% 'ROI28_-36_20_-01.xls'; 'ROI29_06_20_41.xls'; 'ROI30_-43_05_30.xls'; 'ROI31_43_-50_47.xls'; 'ROI32_39_05_31.xls';...
% 'ROI33_-06_29_40.xls'; 'ROI34_32_-04_50.xls'; 'ROI35_-41_-37_44.xls'; 'ROI36_57_-15_34.xls'; 'ROI37_-29_20_09.xls';...
% 'ROI38_49_-22_42.xls'; 'ROI39_-22_09_-05.xls';};

infile = {'R01.xls'; 'R02.xls'; 'R03.xls'; 'R04.xls'; 'R05.xls'; 'R06.xls'; 'R07.xls';...
    'R08.xls'; 'R09.xls'; 'R10.xls'; 'R11.xls'; 'R12.xls'; 'R13.xls'; 'R14.xls';...
    'R15.xls'; 'R16.xls'; 'R17.xls'; 'R18.xls'; 'R19.xls'; 'R20.xls'; 'R21.xls';... 
    'R22.xls'; 'R23.xls'; 'R24.xls'; 'R25.xls'; 'R26.xls'; 'R27.xls'; 'R28.xls';...
    'R29.xls'; 'R30.xls'; 'R31.xls'; 'R32.xls'; 'R33.xls'; 'R34.xls'; 'R35.xls';...
    'R37.xls'; 'R38.xls';};
%'R36.xls';

% infile = {'R01_norm.xls'; 'R02_norm.xls'; 'R03_norm.xls'; 'R04_norm.xls'; 'R05_norm.xls'; 'R06_norm.xls'; 'R07_norm.xls';...
%     'R08_norm.xls'; 'R09_norm.xls'; 'R10_norm.xls'; 'R11_norm.xls'; 'R12_norm.xls'; 'R13_norm.xls'; 'R14_norm.xls';...
%     'R15_norm.xls'; 'R16_norm.xls'; 'R17_norm.xls'; 'R18_norm.xls'; 'R19_norm.xls'; 'R20_norm.xls'; 'R21_norm.xls';... 
%     'R22_norm.xls'; 'R23_norm.xls'; 'R24_norm.xls'; 'R25_norm.xls'; 'R26_norm.xls'; 'R27_norm.xls'; 'R28_norm.xls';...
%     'R29_norm.xls'; 'R30_norm.xls'; 'R31_norm.xls'; 'R32_norm.xls'; 'R33_norm.xls'; 'R34_norm.xls'; 'R35_norm.xls';...
%     'R36_norm.xls'; 'R37_norm.xls'; 'R38_norm.xls';};


%~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%
%~~~        Change these Variables:        ~~~%
%~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%

% Specify format of files (.xls or .txt)
FileForm = '.xls';

% Specify text output file name
FileName = 'output_bootstrap_divtp.txt';

% Define number of regions;
nreg = 37;

% Define number of conditions;
ncond = 5;

% Define number of subjects;
nsubj = 14;

% Define number of subjects used per iteration/sample;
nsamp = 7;

% Define number of clusters;
nclust = 4;
    %What region #s are in each cluster?
    clust{1} = [1 2 3 4 5 6];
    clust{2} = [7 8 9 14 17 22 23 24 26];
    clust{3} = [10 11 12 13 15 16 18 19 20 30 31 32 34 35];
    clust{4} = [21 25 27 28 33 36 37];
    
% Define number of time points in the time course
ntp = 16;

% Define how many points between each tp to interpolate
ntpi = 1000;

% Define minimum timepoint for an onset of signal (1=beginning)
min_O = 2;

% Define maximum timepoint for an onset of signal (ntp=end)
max_O = ntp;

% Define Significance level for ttest (default p=0.05)
tSig = 0.05;

% Define Significance level for anova (default p=0.05)
aSig = 0.001;

% Maximum number of iterations for bootstrap
maxiter=1000;

%~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%
%~~~        Do not change lines below      ~~~%
%~~~          Not even a tiny bit!         ~~~%
%~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%
ncol=ncond*nsubj;                           %# columns in infile

S=1:ncol;                                   %Data array of n columns
Sub=1:nsubj;                                %Data array of n subjects
rgn=1;

tp=1:ntp;
nxi=(ntp*1000)-999;
tpi=1/ntpi;
min_O=(min_O*1000)-999;
max_O=(max_O*1000)-999;
iter = 1;

%output(1:nreg,1:(ncond+1))=0;

%~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%
%~~~ Generate list of 7 random subjects    ~~~%
%~~~ and average tcs for group per cluster ~~~%
%~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%

% Select nsamp subjects randomly without replacement from pool of nsubj
    if exist('randint2') ~= 2
        error('You don''t have the randint2 function. It''s a custom function, so don''t try looking for it. See Josh')
    end
    
    % randint2 allows random integer sampling without replacement, which is kind of important here.
    % Note the full sample for each iteration is with replacement, but within the 14 subjects it's
    % without replacement (i.e., no double/triple/etc subjects in a single iteration).

while iter <= maxiter
    
    [whhsubs] = randint2(1,nsamp,[1:nsubj],'noreplace');
    
    
while rgn<=nreg
%     rgn             % useful for debug
    %~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%
    %~~~            Preallocation              ~~~%
    %~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%
    clear ArrayC
    clear ArrayB
    yi(nxi,:)=0;                            %interpolated time course magnitudes (16000 pts total)
    
    %import data from specified format
    if FileForm=='.xls'
        [ndata]=xlsread (infile{rgn});
    elseif FileForm=='.txt'
        [ndata]=textread (infile{rgn});
    else
    end

    %set up time course matrix
    S=1:ncol;
    xi=1:tpi:ntp;
    tc(:,S)=ndata(:,S+1);

    %SMOOTH TCs with a Savitzky-Golay filter
%     for S=1:ncol;
%         stc(:,S)=smooth(tc(:,S),'sgolay');
%     end

    S=1:ncol;
    %interpolate
    yi(:,S)=interp1(tc(:,S),xi,'linear');


    % Set up arrays for subject #s whhsubs

    a = 1; % Subj # in sample
    b = 1; % Cond
    c = whhsubs(:,a); % Col in region file
    while a ~=0
        ArrayB{a}(:,b)=[yi(:,c+((b-1)*14))];
        %% Debug: c+((b-1)*14)+1
        b = b + 1;
        if a == nsamp && b > ncond
            a = 0;
        elseif a <= nsamp && b > ncond
            a = a + 1;
            c = whhsubs(:,a);
            b = 1;
        end
    end

    % Average ArrayB{1:nsamp} into one timecourse
    a = 1;
    b = 1;
    while a ~=0
        avg{rgn}(:,b)=mean([ArrayB{1}(:,b) ArrayB{2}(:,b) ArrayB{3}(:,b) ArrayB{4}(:,b) ArrayB{5}(:,b) ArrayB{6}(:,b) ArrayB{7}(:,b)], 2);
        b = b+1;
        if b > ncond
            a = 0;
        else
        end
    end

    rgn=rgn+1;

end

% Assemble time course arrays for each cluster
    a = 1;
    b = 1;
    c = 1;
    rgn = 1;
    while a <= nclust
        nrgn_clust{a} = numel(clust{a});
        while rgn <= nreg
            if ismember(rgn,clust{a})==1
                while b <= ncond
                    clust_avg{a}(:,((c-1)*5+b)) = avg{rgn}(:,b);
                    b = b + 1;
                end
                rgn = rgn +1;
                c = c + 1;
                b = 1;
            else
                rgn = rgn + 1;
                b = 1;
            end
        end
        rgn = 1;
        c = 1;
        a = a + 1;
    end

% loop an anova on arrays, 1 pt per region per condition.

    % Assemble initial arrays of conditions (cols) by rgns in cluster (rows)
        a = 1;          % Cluster #
        b = 1;          % Condition #
        rgn = 1;        % rgn is now based off of cluster rgn numbers, not 1-37
        xi = 1;         % interpolated time point
while a <= nclust
    while rgn <= nrgn_clust{a}
        while b <= ncond
            Cond_array{a}{b}(rgn,1) = clust_avg{a}(xi,((rgn-1)*5+b));
            b = b + 1;
        end
        rgn = rgn + 1;
        b = 1;
    end
    rgn = 1;
    a = a + 1;
end

% Find divergence point for each cluster, loop anova on dynamic xi cluster arrays

a = 1;
b = 1;
% c = 1;
rgn = 1;
xi = min_O;
while a <= nclust
    while xi ~=0 && anova1([Cond_array{a}{1:ncond}],[],'off')>aSig
        xi = xi + 1;
        clear Cond_array{a}
        while rgn <= nrgn_clust{a}
            while b <= ncond
                Cond_array{a}{b}(rgn,1) = clust_avg{a}(xi,((rgn-1)*5+b));
                b = b + 1;
            end
            rgn = rgn + 1;
            b = 1;
        end
        rgn = 1;
        if xi>=14999
            xi = 0;
        else
        end
    end
    if xi == 0
        Div_pt(iter,a)=0;
    else
        Div_pt(iter,a)=((xi+999)/1000);
    end
    rgn = 1;
    a = a + 1;
    xi = min_O;
end

% Div_pt      %useful for debug

iter = iter + 1

end


output = Div_pt;

save(FileName,'output','-ASCII');
output
FileName
