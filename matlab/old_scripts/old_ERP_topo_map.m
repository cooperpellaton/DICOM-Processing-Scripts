% J. Tremel (tremeljosh@gmail.com), 2009, University of Pittsburgh

clear all

profile on

% Text data file (make sure there is no string/text in it; just #s)
infile = 'StayOldNewEffects_Export.mul';

% Number of Electrode Belts on head map
nbelts = 11;

% Start time (in ms) of raw data timecourses
t_in = -200;

% Interval to analyze, start time (in ms):
interval_i = 500;

% Interval to analyze, end time (in ms):
interval_f = 700;

% Total # of electrodes (i.e., # columns in infile)
max_elec = 137;

% Number of electrodes in each belt (from center belt outward)
nelec{1} = 1;
nelec{2} = 5;
nelec{3} = 8;
nelec{4} = 12;
nelec{5} = 16;
nelec{6} = 16;
nelec{7} = 20;
nelec{8} = 20;
nelec{9} = 20;
nelec{10} = 20;
nelec{11} = 20;

% Electrode/column number of electrodes in each belt (in order, starting
% with the posterior electrode, moving clockwise)
elec_grp{1} = 1;
elec_grp{2} = [2, 33, 65, 97, 111];
elec_grp{3} = [3, 34, 52, 66, 87, 98, 110, 112];
elec_grp{4} = [4, 0, 51, 53, 64, 75, 86, 88, 109, 114, 113, 0];
elec_grp{5} = [19, 32, 35, 50, 54, 63, 67, 76, 85, 89, 99, 108, 115, 124, 6, 5];
elec_grp{6} = [20, 31, 36, 49, 55, 62, 68, 77, 84, 90, 100, 107, 116, 123, 7, 18];
elec_grp{7} = [21, 30, 37, 45, 48, 56, 61, 69, 74, 78, 83, 91, 96, 101, 106, 117, 122, 125, 8, 17];
elec_grp{8} = [22, 29, 38, 44, 47, 57, 60, 70, 73, 79, 82, 92, 95, 102, 105, 118, 121, 126, 9, 16];
elec_grp{9} = [23, 28, 39, 43, 46, 58, 59, 71, 72, 80, 81, 93, 94, 103, 104, 119, 120, 127, 10, 15];
elec_grp{10} = [27, 40, 42, (max_elec+1), (max_elec+1), (max_elec+1), (max_elec+1), (max_elec+1), (max_elec+1), (max_elec+1), (max_elec+1), (max_elec+1), (max_elec+1), (max_elec+1), (max_elec+1), (max_elec+1), (max_elec+1), 128, 11, 14];
elec_grp{11} = [26, 41, (max_elec+1), (max_elec+1), (max_elec+1), (max_elec+1), (max_elec+1), (max_elec+1), (max_elec+1), (max_elec+1), (max_elec+1), (max_elec+1), (max_elec+1), (max_elec+1), (max_elec+1), (max_elec+1), (max_elec+1), (max_elec+1), 12, 13];

% Number of points to interpolate to within each belt:
ielec = 400;

% Number of belts to interpolate to:
ibelts = 100;
 
% ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%
% ~~~       	Script Below...		    ~~~%
% ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%

mn_belt{nbelts} = 0;

raw = textread(infile);
raw(:,138) = 0;

% Fix time offset (datafile steps by 2 ms)
raw = interp1(raw,0:.5:717,'linear');

interval_i = interval_i - t_in;
interval_f = interval_f - t_in;


a = 1;
while a <= nbelts
    intelec{a} = (nelec{a})/ielec;
    a = a + 1;
end

% average the electrodes for the interval specified
a = 1;
b = 1;

while a <= nbelts
    if min(elec_grp{a}) == 0
        mn_belt{a}(:,1:nelec{a}) = 0;
        while b<=nelec{a}
            if b ~= nelec{a} && elec_grp{a}(:,b) == 0
                mn_belt{a}(:,b) = mean(mean(raw(interval_i:interval_f,[elec_grp{a}(:,b-1) elec_grp{a}(:,b+1)]),2));
                b = b + 1;
            elseif b == nelec{a} && elec_grp{a}(:,b) == 0
                mn_belt{a}(:,b) = mean(mean(raw(interval_i:interval_f,[elec_grp{a}(:,b-1) (max_elec+1)])));
                b = b + 1;
            else
                mn_belt{a}(:,b) = mean(raw(interval_i:interval_f,elec_grp{a}(:,b)));
                b = b + 1;
            end
        end
        a = a + 1;
        b = 1;
    else
        mn_belt{a} = mean(raw(interval_i:interval_f,[elec_grp{a}]));
        a = a + 1;
    end    
end


a = 2;
while a <= nbelts
        mn_belt_m{a}(1,1:(nelec{a}+1)) = 0;
        mn_belt_m{a}(:,1:(nelec{a})) = mn_belt{a};
        mn_belt_m{a}(:,(nelec{a}+1)) = mn_belt{a}(:,1);
        a = a + 1;
%     end
end

% Interpolate the electrode groups in each belt out to ielec points
a = 1;

while a <= nbelts
    if a == 1;
        mn_belt_i{a}(:,1:(ielec+1)) = mn_belt{a};
        a = a + 1;
    else
        mn_belt_i{a} = interp1(mn_belt_m{a}(:,:),1:intelec{a}:(nelec{a}+1),'linear');
        a = a + 1;
    end
end

a = 1;
b = 1;
while a <= nbelts
    mn_belt_ii(a,:) = mn_belt_i{a};
    a = a + 1;
end

% Interpolate the belts to ibelts points
a = 1;
b = 1;
mn_belt_ii_flip = mn_belt_ii';

while a <= nbelts
    while b ~= 0
        mn_belt_if(b,:) = interp1(mn_belt_ii_flip(b,:),1:((nbelts-1)/ibelts):nbelts,'linear');
        if b == (ielec + 1)
            b = 0;
        else
            b = b + 1;
        end
    end
    a = a + 1;
    b = 1;
end


% plot the stuff

n = ibelts + 1;
r = (0:(n-1))'/(n-1);
theta = pi*(-(ielec/2):(ielec/2))/(ielec/2);
x = r*cos(theta);
y = r*sin(theta);
c = mn_belt_if';

pcolor(x, y, c); axis tight; colorbar; shading flat; daspect([1 1 1]); view(270, 90); set(gca,'xtick',[],'ytick',[],'CLim',[-2.5,2.5]);

profile off
